using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Disharp.Client;
using Disharp.Structures;
using Disharp.WebSocket.Payloads;
using Disharp.WebSocket.Payloads.SpecificPayloadData;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Disharp.WebSocket
{
	public class DisharpWebSocketClient
	{
		public DisharpWebSocketClient(DisharpClient client)
		{
			Client = client;
		}

		private DisharpClient Client { get; }
		private WebSocketSharp.WebSocket WebSocketClient { get; set; }
		private Timer HeartbeatTimer { get; set; }
		private int InitialGuilds { get; set; } = 0;

		internal dynamic Sequence { get; set; }
		internal string SessionId { get; set; }

		internal async Task ConnectAsync()
		{
			WebSocketClient = new WebSocketSharp.WebSocket(
				$"{Client.ClientOptions.WsOptions.GatewayUrl}?v={Client.ClientOptions.WsOptions.GatewayVersion}&encoding={Client.ClientOptions.WsOptions.EncodingType}");

			WebSocketClient.OnMessage += async (sender, msg) =>
			{
				await _onWsMessage(sender, msg);
			};

			WebSocketClient.OnClose += _onWsClose;

			WebSocketClient.Connect();
		}

		private void _onWsClose(object sender, CloseEventArgs args)
		{
			Console.WriteLine(args.Reason);
			Console.WriteLine(args.Code);
		}

		private async Task _onWsMessage(object sender, MessageEventArgs msg)
		{
			var serializedPayload = msg.Data;

			var deserializedPayload = JsonConvert.DeserializeObject<DiscordGatewayPayload<dynamic>>(serializedPayload,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});

			if (deserializedPayload.S != 0) Sequence = deserializedPayload.S;

			switch (deserializedPayload.Op)
			{
				case 0:
				{
					switch (deserializedPayload.T)
					{
						case "READY":
						{
							var readyPayload =
								JsonConvert.DeserializeObject<DiscordGatewayPayload<ReadyPayload>>(
									JsonConvert.SerializeObject(deserializedPayload));

							Client.ClientUser = new ClientUser
							{
								Avatar = readyPayload.D.User.Avatar,
								Bot = readyPayload.D.User.Bot,
								Discriminator = readyPayload.D.User.Discriminator,
								Flags = readyPayload.D.User.Flags,
								Id = readyPayload.D.User.Id,
								Username = readyPayload.D.User.Username
							};

							SessionId = readyPayload.D.SessionId;

							HeartbeatTimer.Start();
							HeartbeatTimer.Elapsed += Heartbeat;

							InitialGuilds = readyPayload.D.UnavailableGuilds.Length;
							
							break;
						}
						case "GUILD_CREATE":
						{
							var guildPayload =
								JsonConvert.DeserializeObject<DiscordGatewayPayload<GuildCreatePayload>>(
									JsonConvert.SerializeObject(deserializedPayload));

							if (guildPayload.D.Lazy)
							{
								InitialGuilds--;
								await Client.Guilds.GetOrCreateAsync(guildPayload.D.Id, async () => JsonConvert.DeserializeObject<DiscordGatewayPayload<Guild>>(JsonConvert.SerializeObject(guildPayload)).D);
								if (InitialGuilds == 0)
								{
									Client.ReadyEvent(EventArgs.Empty);
								}
							}
							else
							{
								var seeIfGuildWasUnavailable = await Client.UnavailableGuilds.GetAsync(guildPayload.D.Id);

								if (seeIfGuildWasUnavailable == null)
								{
									var guild = await Client.Guilds.GetOrCreateAsync(guildPayload.D.Id, async () => JsonConvert.DeserializeObject<DiscordGatewayPayload<Guild>>(JsonConvert.SerializeObject(guildPayload)).D);
									Client.GuildCreateEvent(guild);
								}
								else
								{
									var guild = await Client.Guilds.GetOrCreateAsync(guildPayload.D.Id, async () => JsonConvert.DeserializeObject<DiscordGatewayPayload<Guild>>(JsonConvert.SerializeObject(guildPayload)).D);
									await Client.UnavailableGuilds.DeleteAsync(guildPayload.D.Id);
									Client.GuildAvailableEvent(guild);
								}
							}

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

					if (HeartbeatTimer != null)
					{
						HeartbeatTimer.Elapsed -= Heartbeat;
						HeartbeatTimer.Stop();
						HeartbeatTimer.Dispose();
					}

					HeartbeatTimer = new Timer(helloPayload.D.HeartbeatInterval);
					HeartbeatTimer.AutoReset = true;

					var identifyPayload = new DiscordGatewayPayload<IdentifyPayload>
					{
						Op = 2,
						D = new IdentifyPayload
						{
							Token = Client.Token,
							Properties = new IdentifyPropertiesPayload
							{
								Browser = "Disharp",
								Device = "Disharp",
								Os = Environment.OSVersion.Platform.ToString()
							},
							Compress = false,
							LargeThreshold = Client.ClientOptions.LargeThreshold,
							Presence = Client.ClientOptions.WsOptions.Presence,
							Intents = Client.ClientOptions.WsOptions.Intents
						}
					};

					Send(identifyPayload);
					break;
				}
			}

			Console.WriteLine(JsonConvert.SerializeObject(deserializedPayload));
		}

		private void Heartbeat(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("Sent Heartbeat!");
			var heartBeatPayload = new DiscordGatewayPayload<dynamic>
			{
				Op = 1,
				D = Sequence
			};

			Send(heartBeatPayload);
		}

		private void Send(object data)
		{
			var payload = JsonConvert.SerializeObject(data);

			WebSocketClient.Send(payload);
		}
	}
}