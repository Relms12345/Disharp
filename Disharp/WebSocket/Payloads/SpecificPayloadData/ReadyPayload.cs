using Disharp.Structures;
using Newtonsoft.Json;

namespace Disharp.WebSocket.Payloads.SpecificPayloadData
{
	public class ReadyPayload
	{
		[JsonProperty("user")] public UserPayload User { get; set; }

		[JsonProperty("session_id")] public string SessionId { get; set; }

		[JsonProperty("guilds")] public UnavailableGuild[] UnavailableGuilds { get; set; }
	}
}