using System;

namespace Disharp.Rest
{
	public class DisharpRestClientOptions
	{
		public Uri ApiUrl { get; set; } = new Uri("https://discord.com/api/");
		public Uri CdnUrl { get; set; } = new Uri("https://cdn.discordapp.com/");
		public int Version { get; set; } = 7;
		public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
		public int Offset { get; set; } = 50;
		public int Retries { get; set; } = 10;
	}
}