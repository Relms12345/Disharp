using Disharp.Cache;
using Disharp.Rest;
using Disharp.WebSocket;

namespace Disharp.Client
{
	public sealed class DisharpClientOptions
	{
		public DisharpClientOptions()
		{
		}

		public DisharpClientOptions(DisharpClientOptions config)
		{
			WsOptions = config.WsOptions;
			RestOptions = config.RestOptions;
			CacheOptions = config.CacheOptions;
		}

		public DisharpWebSocketClientOptions WsOptions { get; set; } = new DisharpWebSocketClientOptions();
		public DisharpRestClientOptions RestOptions { get; set; } = new DisharpRestClientOptions();
		public DisharpCacheOptions CacheOptions { get; set; } = new DisharpCacheOptions();
		public int LargeThreshold { get; set; } = 50;
	}
}