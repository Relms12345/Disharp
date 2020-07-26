using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disharp.Constants;
using Disharp.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disharp.Core
{
	public class DisharpClient
	{
		public DisharpClient(DisharpClientOptions options)
		{
			DefaultClientOptions.SetDefaultOptions();

			var DefaultOptions = JObject.Parse(JsonConvert.SerializeObject(DefaultClientOptions.GetDefaultOptions()));
			var UserOptions = JObject.Parse(JsonConvert.SerializeObject(options));
			
			DefaultOptions.Merge(UserOptions, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Union
			});

			Options = JsonConvert.DeserializeObject<DisharpClientOptions>(DefaultOptions.ToString());
		}

		public static DisharpClientOptions Options { get; set; }
		public static DisharpWebSocketClient Ws { get; set; }

		private static string _token { get; set; }
		private static TokenType _tokenType { get; set; }

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			_token = token;
			_tokenType = tokenType;

			Ws = new DisharpWebSocketClient(new DisharpWebSocketClientOptions
			{
				Compress = Options.WsOptions.Compress,
				EncodingType = Options.WsOptions.EncodingType,
				BaseGatewayUrl = Options.WsOptions.BaseGatewayUrl,
				BaseGatewayVersion = Options.WsOptions.BaseGatewayVersion
			});

			await Ws.ConnectAsync(_tokenType, _token);
		}
	}
}