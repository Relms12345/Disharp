using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disharp.Cache;
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
		public Cache<UnavailableGuild> UnavailableGuilds { get; private set; }
		public Cache<Guild> Guilds { get; private set; }

		internal TokenType TokenType { get; set; }
		internal string Token { get; set; }

		public event EventHandler Ready;
		public event EventHandler<Guild> GuildCreate;
		public event EventHandler<Guild> GuildAvailable; 

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			TokenType = tokenType;
			Token = token;

			Ws = new DisharpWebSocketClient(this);
			Rest = new DisharpRestClient(this);
			
			UnavailableGuilds = new Cache<UnavailableGuild>(this);
			Guilds = new Cache<Guild>(this);

			await Ws.ConnectAsync();
		}

		internal void ReadyEvent(EventArgs e)
		{
			var handler = Ready;
			handler?.Invoke(this, e);
		}
		
		internal void GuildCreateEvent(Guild g)
		{
			var handler = GuildCreate;
			handler?.Invoke(this, g);
		}
		
		internal void GuildAvailableEvent(Guild g)
		{
			var handler = GuildAvailable;
			handler?.Invoke(this, g);
		}
	}
}