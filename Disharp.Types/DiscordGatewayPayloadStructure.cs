namespace Disharp.Types
{
	public class DiscordGatewayPayloadStructure
	{
		public int op { get; set; }
		public dynamic d { get; set; }
		public int s { get; set; }
		public string t { get; set; }
	}
}