using System;
using System.Threading.Tasks;
using System.Timers;
using Disharp.Client;
using Disharp.Structures;
using Disharp.Structures.SpecificPayloadData;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Disharp.WebSocket
{
	public class DisharpWebSocketClient
	{
		public DisharpWebSocketClient(DisharpClient client)
		{
			_client = client;
		}

		private DisharpClient _client { get; }
		private WebSocketSharp.WebSocket _webSocketClient { get; set; }
		private Timer heartbeatTimer { get; set; }

		internal dynamic _sequence { get; set; } = null;

		internal async Task ConnectAsync()
		{
			_webSocketClient = new WebSocketSharp.WebSocket(
				$"{_client.ClientOptions.WsOptions.GatewayURL}?v={_client.ClientOptions.WsOptions.GatewayVersion}&encoding={_client.ClientOptions.WsOptions.EncodingType}");

			_webSocketClient.OnMessage += _onWsMessage;

			_webSocketClient.OnClose += _onWsClose;

			_webSocketClient.Connect();
		}

		private void _onWsClose(object sender, CloseEventArgs args)
		{
			Console.WriteLine(args.Reason);
			Console.WriteLine(args.Code);
		}

		private void _onWsMessage(object sender, MessageEventArgs msg)
		{
			var serializedPayload = msg.Data;

			var deserializedPayload = JsonConvert.DeserializeObject<DiscordGatewayPayload<dynamic>>(serializedPayload,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});

			if (deserializedPayload.S != 0 || deserializedPayload.S != null)
			{
				_sequence = deserializedPayload.S;
			}
			
			switch (deserializedPayload.Op)
			{
				case 0:
				{
					switch (deserializedPayload.T)
					{
						case "READY":
						{
							
							heartbeatTimer.Start();
							heartbeatTimer.Elapsed += heartbeat;
							break;
						}
					}
					break;
				}
				case 10:
				{
					var helloPayload =
						JsonConvert.DeserializeObject<DiscordGatewayPayload<HelloPayload>>(
							JsonConvert.SerializeObject(deserializedPayload));

					if (heartbeatTimer != null)
					{
						heartbeatTimer.Elapsed -= heartbeat;
						heartbeatTimer.Stop();
						heartbeatTimer.Dispose();
					}
				
					heartbeatTimer = new Timer(helloPayload.D.HeartbeatInterval);
					heartbeatTimer.AutoReset = true;

					var identifyPayload = new DiscordGatewayPartialPayload<IdentifyPayload>
					{
						Op = 2,
						D = new IdentifyPayload
						{
							Token = _client._token,
							Properties = new IdentifyPropertiesPayload
							{
								Browser = "Disharp",
								Device = "Disharp",
								Os = Environment.OSVersion.Platform.ToString()
							},
							Compress = false,
							LargeThreshold = _client.ClientOptions.LargeThreshold,
							Presence = _client.ClientOptions.WsOptions.Presence,
							Intents = _client.ClientOptions.WsOptions.Intents
						}
					};
				
					Send(identifyPayload);
					break;
				}
			}

			Console.WriteLine(JsonConvert.SerializeObject(deserializedPayload, Formatting.Indented));
		}

		private void heartbeat(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("Sent Heartbeat!");
			var heartBeatPayload = new DiscordGatewayPartialPayload<dynamic>
			{
				Op = 1,
				D = _sequence
			};
			
			Send(heartBeatPayload);
		}

		private void Send(object data)
		{
			var payload = JsonConvert.SerializeObject(data);
			
			Console.WriteLine(payload);
			
			_webSocketClient.Send(payload);
		}
	}
}