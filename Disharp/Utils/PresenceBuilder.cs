using System;
using Disharp.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disharp.Utils
{
	public class PresenceBuilder
	{
		[JsonProperty("since")]
		public long Since { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
		
		[JsonProperty("game")]
		public PresenceGame Game { get; set; }
		
		[JsonProperty("status")]
		public StatusType Status { get; set; }
		
		[JsonProperty("afk")]
		public bool Afk { get; set; }
	}

	public class PresenceGame
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		
		[JsonProperty("type")]
		public GameType Type { get; set; }
		
		[JsonProperty("url")]
		public Uri Url { get; set; }
	}
}