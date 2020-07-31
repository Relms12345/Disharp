using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Disharp.Client;
using Disharp.Types;
using Disharp.Utils;
using Microsoft.QueryStringDotNET;
using Newtonsoft.Json;
using RestSharp;

namespace Disharp.Rest
{
	public class RestManager
	{
		public RestManager(DisharpClient client, DisharpRestClient rest)
		{
			_client = client;
			_token = _client._token;
			_sweeper = new Timer(300000);

			RestClient = new RestClient();
			Rest = rest;

			_sweeper.AutoReset = true;
			_sweeper.Start();
			_sweeper.Elapsed += OnSweeperOnElapsed;
		}

		internal DisharpRestClient Rest { get; set; }
		internal RestClient RestClient { get; set; }

		public dynamic GlobalTimeout { get; set; } = null;
		public Dictionary<string, string> Hashes { get; set; } = new Dictionary<string, string>();

		private Dictionary<string, RequestHandler> _queues { get; } = new Dictionary<string, RequestHandler>();
		private string _token { get; }
		private DisharpClient _client { get; }
		private Timer _sweeper { get; }

		private void OnSweeperOnElapsed(object sender, EventArgs args)
		{
			foreach (var queue in _queues) queue.Value.Inactive();
		}

		public Task<dynamic> QueueRequest(RestReq request)
		{
			var routeId = GenerateRouteIdentifiers(request.Endpoint, request.Method.ToString());
			var tryGetVal = Hashes.TryGetValue($"{request.Method.ToString()}-${routeId.Route}", out var hash);

			if (tryGetVal == false) hash = $"UnknownHash({routeId.Route})";

			_queues.TryGetValue($"{hash}:{routeId.MajorParameter}", out var queue);

			if (queue == null)
			{
				CreateQueue(hash, routeId.MajorParameter);

				_queues.TryGetValue($"{hash}:{routeId.MajorParameter}", out queue);
			}

			var res = ResolveRequest(request);

			var resolveOptions = new RestReq
			{
				Auth = request.Auth,
				Data = res.Options.Body,
				Endpoint = request.Endpoint,
				Files = res.Options.Files,
				Headers = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(
					JsonConvert.SerializeObject(res.Options.Headers)),
				Method = res.Options.Method,
				Query = request.Query,
				Reason = request.Reason
			};

			return queue.Push(routeId, res.Url, resolveOptions);
		}

		private RequestHandler CreateQueue(string hash, string majorParameter)
		{
			var queue = new RequestHandler(_client, this, hash, majorParameter);
			_queues.Add(queue.Id, queue);
			return queue;
		}

		private ResolveRequestStruct ResolveRequest(RestReq request)
		{
			var query = new QueryString();
			var querystring = "";

			if (request.Query != null)
			{
				foreach (var (key, value) in request.Query)
					if (value != null)
						query.Add(key, value);

				querystring = $"?{query}";
			}

			var url = querystring.Length != 0
				? $"{_client.ClientOptions.RestOptions.ApiUrl}v{_client.ClientOptions.RestOptions.Version}/{request.Endpoint}/?{querystring}"
				: $"{_client.ClientOptions.RestOptions.ApiUrl}v{_client.ClientOptions.RestOptions.Version}/{request.Endpoint}";

			var headers = new HeaderOptions
			{
				UserAgent = "DiscordBot ($Disharp $0.0.1)",
				XRateLimitPrecision = "second"
			};

			if (request.Auth)
			{
				if (_token == null)
					throw new NullReferenceException(
						"No bot token has been provided, and is required for the action you are trying to do.");
				// Provide authorization headers
				headers.Authorization = $"{_client._tokenType} {_token}";
			}

			if (request.Reason != null) headers.XAuditLogReason = Uri.EscapeDataString(request.Reason);

			var body = JsonConvert.SerializeObject(request.Data);
			headers.ContentType = "application/json";

			var options = new HttpOptions
			{
				Method = request.Method,
				Headers = new HeaderOptions
				{
					Authorization = headers.Authorization,
					ContentType = headers.ContentType,
					UserAgent = headers.UserAgent,
					XAuditLogReason = headers.XAuditLogReason,
					XRateLimitPrecision = headers.XRateLimitPrecision
				},
				Body = body,
				Files = request.Files
			};

			// Return the data needed for node-fetch
			return new ResolveRequestStruct
			{
				Url = url,
				Options = options
			};
		}

		private static RouteIdentifier GenerateRouteIdentifiers(string endpoint, string method)
		{
			var result = new Regex(@"^\/(?:channels|guilds|webhooks)\/(\d{16,19})").Matches(endpoint);
			var majorParameter = result.Count != 0 ? result[1].ToString() : "global";
			var baseRoute = endpoint.Replace(new Regex(@"\d{16,19}").ToString(), ":id");

			var exceptions = "";

			if (method == "delete" && baseRoute == "/channels/:id/messages/:id")
			{
				var id = new Regex(@"\\d{16,19}$").Match(endpoint);
				var snowflake = Snowflake.FromSnowflake(id.ToString());
				if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - snowflake.ToUnixTimeMilliseconds() > 1000 * 60 * 60 * 24 * 14)
					exceptions += "[Delete Old Message]";
			}

			return new RouteIdentifier
			{
				Route = $"{baseRoute}{exceptions}",
				MajorParameter = majorParameter
			};
		}

		internal struct ResolveRequestStruct
		{
			public string Url { get; set; }
			public HttpOptions Options { get; set; }
		}

		internal struct HeaderOptions
		{
			public string UserAgent { get; set; }

			[JsonProperty("X-RateLimit-Precision")]
			public string XRateLimitPrecision { get; set; }

			public string Authorization { get; set; }

			[JsonProperty("X-Audit-Log-Reason")] public string XAuditLogReason { get; set; }

			[JsonProperty("Content-Type")] public string ContentType { get; set; }
		}

		internal struct HttpOptions
		{
			public Method Method { get; set; }
			public HeaderOptions Headers { get; set; }
			public dynamic Body { get; set; }
			public FileParameter[] Files { get; set; }
		}
	}
}