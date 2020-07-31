using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disharp.Constants;
using Disharp.Rest;
using Disharp.Structures;
using Disharp.WebSocket;

namespace Disharp.Client
{
	public sealed class DisharpClient
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

		internal TokenType TokenType { get; set; }
		internal string Token { get; set; }

		public event EventHandler Ready;

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			TokenType = tokenType;
			Token = token;

			Ws = new DisharpWebSocketClient(this);
			Rest = new DisharpRestClient(this);

			await Ws.ConnectAsync();
		}

		internal void ReadyEvent(EventArgs e)
		{
			var handler = Ready;
			handler?.Invoke(this, e);
		}
	}
}