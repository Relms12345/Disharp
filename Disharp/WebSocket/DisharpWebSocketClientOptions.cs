using System;
using Disharp.Constants;
using Disharp.Utils;

namespace Disharp.WebSocket
{
	public sealed class DisharpWebSocketClientOptions
	{
		public Uri GatewayURL { get; set; } = new Uri("wss://gateway.discord.gg/");

		public int GatewayVersion { get; set; } = 7;

		// public bool Compress { get; set; } = true;
		public EncodingType EncodingType { get; set; } = EncodingType.Json;
		public PresenceBuilder Presence { get; set; } = new PresenceBuilder();

		public int Intents { get; set; } =
			new IntentsBuilder(new[] {Constants.Intents.Guilds, Constants.Intents.GuildMessages}).Intent;
	}
}