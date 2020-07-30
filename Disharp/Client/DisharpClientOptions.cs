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
		}

		public DisharpWebSocketClientOptions WsOptions { get; set; } = new DisharpWebSocketClientOptions();

		public int LargeThreshold { get; set; } = 50;
	}
}