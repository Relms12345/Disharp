using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disharp.Client;
using Disharp.Rest.Queue;
using Disharp.Types;
using Newtonsoft.Json;
using RestSharp;
using RSG;
using Timer = System.Timers.Timer;

namespace Disharp.Rest
{
	public class RequestHandler
	{
		public RequestHandler(DisharpClient client, RestManager manager, string hash, string majorParameter)
		{
			Id = $"{hash}:{majorParameter}";

			_manager = manager;
			_client = client;
			Hash = hash;
		}

		public string Id { get; set; }

		private long Reset { get; set; } = -1;
		private long Remaining { get; set; } = 1;
		private long Limit { get; set; } = long.MaxValue;
		private string Hash { get; }
		private AsyncQueue Queue { get; } = new AsyncQueue();
		private RestManager _manager { get; }
		private DisharpClient _client { get; }

		public bool Inactive()
		{
			return Queue.Remaining() == 0 && !Limited();
		}

		private bool Limited()
		{
			return Remaining <= 0 && new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() < Reset;
		}

		private long TimeToReset()
		{
			return Reset - Convert.ToInt64(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds());
		}

		public async Task<dynamic> Push(RouteIdentifier routeId, string url, RestReq options)
		{
			Queue.Wait();
			try
			{
				var temp = _manager.GlobalTimeout;
				if (Limited())
				{
					// Let library users know they have hit a ratelimit
					// this._manager.Rest.emit(RESTManagerEvents.Ratelimited, {
					// 	timeToReset: this.TimeToReset,
					// 	limit: this.limit,
					// 	method: options.method,
					// 	hash: this.hash,
					// 	route: routeID.route,
					// 	majorParameter: this.majorParameter
					// });

					Console.WriteLine("R8 LIMIT!");

					await Task.Delay(Convert.ToInt32(TimeToReset()));
				}

				return await MakeRequest(routeId, url, options);
			}
			finally
			{
				Queue.Shift();
			}
		}

		private async Task<dynamic> MakeRequest(RouteIdentifier routeId, string url, RestReq options, int retries = 0)
		{
			var controller = new CancellationTokenSource();

			var abortTimer = new Timer(_client.ClientOptions.RestOptions.Timeout.TotalMilliseconds)
			{
				AutoReset = false
			};

			abortTimer.Start();
			abortTimer.Elapsed += (sender, args) =>
			{
				controller.Cancel();
				abortTimer.Dispose();
			};

			IRestResponse res;

			try
			{
				var restRequest = new RestRequest(url, DataFormat.Json)
				{
					Method = options.Method
				};

				if (options.Headers != null)
					foreach (var header in options.Headers)
						restRequest.AddHeader(header.Key, header.Value);

				if (options.Files != null)
					foreach (var file in options.Files)
						restRequest.AddFile(file.Name, file.Writer, file.FileName, file.ContentLength, file.ContentType);

				if (options.Data != null)
				{
					restRequest.AddJsonBody(options.Data);
				}

				switch (options.Method)
				{
					case Method.GET:
						res = _manager.RestClient.Get(restRequest);
						break;
					case Method.POST:
						res = _manager.RestClient.Post(restRequest);
						break;
					default:
						res = _manager.RestClient.Get(restRequest);
						break;
				}
				
				
			}
			catch (Exception error)
			{
				Console.WriteLine(JsonConvert.SerializeObject(error));
				// if (error.Source == 'AbortError' && retries !== this.manager.options.retries) return this.makeRequest(routeID, url, options, ++retries);
				throw;
			}
			finally
			{
				abortTimer.Stop();
				abortTimer.Dispose();
			}

			var retryAfter = 0;
			
			Console.WriteLine(JsonConvert.SerializeObject(res.Headers, Formatting.Indented));

			var limit = res.Headers.ToArray().ToList().Find(x => x.Name == "x-ratelimit-limit");
			var remaining = res.Headers.ToArray().ToList().Find(x => x.Name == "x-ratelimit-remaining");
			var reset = res.Headers.ToArray().ToList().Find(x => x.Name == "x-ratelimit-reset-after");
			var hash = res.Headers.ToArray().ToList().Find(x => x.Name == "x-ratelimit-bucket");
			var retry = res.Headers.ToArray().ToList().Find(x => x.Name == "Retry-After");

			var cloudflare = res.Headers.ToArray().ToList().Find(x => x.Name == "Via") != null;

			Limit = limit != null ? Convert.ToInt32(limit.Value) : int.MaxValue;
			Remaining = remaining != null ? Convert.ToInt32(remaining.Value) : int.MaxValue;
			Reset = reset != null
				? Convert.ToInt32(reset.Value) * 1000 + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + _client.ClientOptions.RestOptions.Offset
				: new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();

			if (retry != null)
				retryAfter = Convert.ToInt32(retry.Value) * (cloudflare ? 1000 : 1) +
				             _client.ClientOptions.RestOptions.Offset;

			if (hash != null && Convert.ToString(hash.Value) != Hash)
				// this.manager.rest.emit(RESTManagerEvents.Debug, `Bucket hash update: ${this.hash} => ${hash} for ${options.method}-${routeID.route}`);
				if (!_manager.Hashes.ContainsKey($"{options.Method}-${routeId.Route}"))
					_manager.Hashes.Add($"{options.Method}-${routeId.Route}", Convert.ToString(hash.Value));

			if (res.Headers.ToArray().ToList().Find(x => x.Name == "x-ratelimit-global") != null)
			{
				_manager.GlobalTimeout = new Promise();
				Thread.Sleep(retryAfter);
				_manager.GlobalTimeout = null;
			}

			if (res.IsSuccessful) return ParseResponse(res);

			if (res.StatusCode == HttpStatusCode.TooManyRequests)
			{
				// this.manager.rest.emit(RESTManagerEvents.Debug, `429 hit on route: ${routeID.route}\nRetrying after: ${retryAfter}ms`);
				Console.WriteLine("R8 LIMIT!");
				await Task.Delay(retryAfter);
				return await MakeRequest(routeId, url, options, retries);
			}

			if (res.StatusCode == HttpStatusCode.InternalServerError)
			{
				if (retries != _client.ClientOptions.RestOptions.Retries)
					return await MakeRequest(routeId, url, options, ++retries);

				Console.WriteLine("ERROR BITCH!");
				// throw new HTTPError(res.statusText, res.constructor.name, res.status, options.method as string, url);
			}
			else
			{
				if (res.StatusCode == HttpStatusCode.BadRequest)
				{
					var data = await ParseResponse(res);
					Console.WriteLine("MALFORMED DATA!");
					// throw new DiscordAPIError(data.message, data.code, res.status, options.method as string, url);
				}

				return null;
			}

			return null;
		}

		private static dynamic ParseResponse(IRestResponse res)
		{
			if (res.Headers.ToArray().ToList().Find(x => x.Name == "Content-Type").Value.ToString()
				.StartsWith("application/json"))
				return res.Content;

			return Encoding.UTF8.GetBytes(res.Content);
		}
	}
}