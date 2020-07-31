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
		}

		public DisharpWebSocketClientOptions WsOptions { get; set; } = new DisharpWebSocketClientOptions();
		public DisharpRestClientOptions RestOptions { get; set; } = new DisharpRestClientOptions();
		public int LargeThreshold { get; set; } = 50;
	}
}