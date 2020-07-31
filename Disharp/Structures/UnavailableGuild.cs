using Newtonsoft.Json;

namespace Disharp.Structures
{
	public class UnavailableGuild
	{
		[JsonProperty("unavailable")] public bool Unavailable { get; set; }

		[JsonProperty("id")] public string Id { get; set; }
	}
}