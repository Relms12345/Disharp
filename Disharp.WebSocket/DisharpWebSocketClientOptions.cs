using System;
using Disharp.Constants;

namespace Disharp.WebSocket
{
	public class DisharpWebSocketClientOptions
	{
		public int BaseGatewayVersion { get; set; } = 7;
		public bool Compress { get; set; }
		public EncodingType EncodingType { get; set; }
		public Uri BaseGatewayUrl { get; set; }
	}
}