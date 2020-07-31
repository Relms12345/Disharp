using Newtonsoft.Json;

namespace Disharp.WebSocket.Payloads.SpecificPayloadData
{
	public class HelloPayload
	{
		[JsonProperty("heartbeat_interval")] public int HeartbeatInterval { get; set; }
	}
}