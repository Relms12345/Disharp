using Newtonsoft.Json;

namespace Disharp.WebSocket.Payloads.SpecificPayloadData
{
	public class GuildDeletePayload
	{
		[JsonProperty("unavailable")] public bool Unavailable { get; set; }

		[JsonProperty("id")] public string Id { get; set; }
	}
}