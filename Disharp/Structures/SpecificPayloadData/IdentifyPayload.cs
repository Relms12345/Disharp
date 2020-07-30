using Disharp.Utils;
using Newtonsoft.Json;

namespace Disharp.Structures.SpecificPayloadData
{
	public class IdentifyPayload
	{
		[JsonProperty("token")]
		public string Token { get; set; }
		
		[JsonProperty("properties")]
		public IdentifyPropertiesPayload Properties { get; set; }
		
		[JsonProperty("compress")]
		public bool Compress { get; set; }
		
		[JsonProperty("large_threshold")]
		public int LargeThreshold { get; set; }
		
		[JsonProperty("presence")]
		public PresenceBuilder Presence { get; set; }
		
		[JsonProperty("intents")]
		public int Intents { get; set; }
	}

	public class IdentifyPropertiesPayload
	{
		[JsonProperty("$os")]
		public string Os { get; set; }
		
		[JsonProperty("$browser")]
		public string Browser { get; set; }
		
		[JsonProperty("$device")]
		public string Device { get; set; }
	}
}