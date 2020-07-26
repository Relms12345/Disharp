using System;
using System.Threading.Tasks;
using Disharp.Constants;
using Disharp.Types;
using Disharp.Utils;
using Newtonsoft.Json;
using WebSocketSharp;
using Logger = Disharp.Utils.Logger;

namespace Disharp.WebSocket
{
	public class DisharpWebSocketClient
	{
		public DisharpWebSocketClient(DisharpWebSocketClientOptions socketClientOptions)
		{
			_socketClientOptions = socketClientOptions;
		}

		public static WebSocketSharp.WebSocket WebSocketServer { get; set; }

		private static DisharpWebSocketClientOptions _socketClientOptions { get; set; }

		public async Task ConnectAsync(TokenType tokenType, string token)
		{
			WebSocketServer = new WebSocketSharp.WebSocket(
				$"{_socketClientOptions.BaseGatewayUrl}?v={_socketClientOptions.BaseGatewayVersion}&encoding={_socketClientOptions.EncodingType}{(_socketClientOptions.Compress ? "&compress=zlib-stream" : "")}");

			WebSocketServer.OnMessage += OnWSMessage;

			WebSocketServer.Connect();
		}

		private void OnWSMessage(object sender, MessageEventArgs msg)
		{
			var serializedPayload = _socketClientOptions.Compress ? msg.RawData.DecompressStream() : msg.Data;

			var deserializedPayload = JsonConvert.DeserializeObject<DiscordGatewayPayloadStructure>(serializedPayload, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			});

			Logger.Log().Debug(serializedPayload);
		}
	}
}