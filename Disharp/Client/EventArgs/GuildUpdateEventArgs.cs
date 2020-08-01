using Disharp.Structures;

namespace Disharp.Client.EventArgs
{
	public class GuildUpdateEventArgs : System.EventArgs
	{
		public Guild OldGuild { get; set; }
		public Guild NewGuild { get; set; }
	}
}