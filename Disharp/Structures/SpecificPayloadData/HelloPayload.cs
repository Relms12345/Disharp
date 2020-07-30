using Newtonsoft.Json;

namespace Disharp.Structures.SpecificPayloadData
{
	public class HelloPayload
	{
		[JsonProperty("heartbeat_interval")] public int HeartbeatInterval { get; set; }
	}
}