using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disharp.Constants;
using Disharp.Rest;
using Disharp.Structures;
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
		public DisharpRestClient Rest { get; set; }
		public DisharpClientOptions ClientOptions { get; set; }

		public ClientUser ClientUser { get; set; }

		public Dictionary<string, UnavailableGuild> UnavailableGuilds { get; set; } =
			new Dictionary<string, UnavailableGuild>();

		internal TokenType _tokenType { get; set; }
		internal string _token { get; set; }

		public event EventHandler Ready;

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			_tokenType = tokenType;
			_token = token;

			Ws = new DisharpWebSocketClient(this);
			Rest = new DisharpRestClient(this);

			await Ws.ConnectAsync();
		}

		protected internal virtual void ReadyEvent(EventArgs e)
		{
			var handler = Ready;
			handler?.Invoke(this, e);
		}
	}
}