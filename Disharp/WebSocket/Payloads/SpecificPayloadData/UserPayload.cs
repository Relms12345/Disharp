using Newtonsoft.Json;

namespace Disharp.WebSocket.Payloads.SpecificPayloadData
{
	public class UserPayload
	{
		[JsonProperty("username")] public string Username { get; set; }

		[JsonProperty("id")] public string Id { get; set; }

		[JsonProperty("flags")] public int Flags { get; set; }

		[JsonProperty("discriminator")] public string Discriminator { get; set; }

		[JsonProperty("bot")] public bool Bot { get; set; }

		[JsonProperty("avatar")] public string Avatar { get; set; }
	}
}