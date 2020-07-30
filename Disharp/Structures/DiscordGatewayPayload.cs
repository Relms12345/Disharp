using Newtonsoft.Json;

namespace Disharp.Structures
{
	public class DiscordGatewayPayload<Type> where Type : notnull
	{
		[JsonProperty("op")] public int Op { get; set; }

		[JsonProperty("d")] public Type D { get; set; }

		[JsonProperty("s")] public int S { get; set; }

		[JsonProperty("t")] public string T { get; set; }
	}
	
	public class DiscordGatewayPartialPayload<Type> where Type : notnull
	{
		[JsonProperty("op")] public int Op { get; set; }

		[JsonProperty("d")] public Type D { get; set; }
	}
}