using System.Threading.Tasks;
using Disharp.Constants;
using Disharp.WebSocket;

namespace Disharp.Client
{
	public class DisharpClient
	{
		public DisharpClient(DisharpClientOptions clientOptions)
		{
			ClientOptions = new DisharpClientOptions(clientOptions);
		}

		public DisharpWebSocketClient Ws { get; set; }
		public DisharpClientOptions ClientOptions { get; set; }

		internal TokenType _tokenType { get; set; }
		internal string _token { get; set; }

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			_tokenType = tokenType;
			_token = token;

			Ws = new DisharpWebSocketClient(this);

			await Ws.ConnectAsync();
		}
	}
}