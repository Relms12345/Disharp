using System;
using System.Threading.Tasks;
using Disharp.Cache;
using Disharp.Client.EventArgs;
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
		public Cache<Guild> UnavailableGuilds { get; set; }
		public Cache<Guild> Guilds { get; set; }

		internal TokenType TokenType { get; set; }
		internal string Token { get; set; }

		public event EventHandler Ready;
		
		public event EventHandler<Guild> GuildCreate;
		public event EventHandler<GuildUpdateEventArgs> GuildUpdate;
		public event EventHandler<Guild> GuildAvailable;
		public event EventHandler<Guild> GuildUnavailable;
		public event EventHandler<Guild> GuildDelete;

		public async Task LoginAsync(TokenType tokenType, string token)
		{
			TokenType = tokenType;
			Token = token;

			Ws = new DisharpWebSocketClient(this);
			Rest = new DisharpRestClient(this);

			UnavailableGuilds = new Cache<Guild>(this);
			Guilds = new Cache<Guild>(this);

			await Ws.ConnectAsync();
		}

		internal void ReadyEvent(System.EventArgs e)
		{
			var handler = Ready;
			handler?.Invoke(this, e);
		}

		internal void GuildCreateEvent(Guild g)
		{
			var handler = GuildCreate;
			handler?.Invoke(this, g);
		}
		
		internal void GuildUpdateEvent(GuildUpdateEventArgs e)
		{
			var handler = GuildUpdate;
			handler?.Invoke(this, e);
		}

		internal void GuildAvailableEvent(Guild g)
		{
			var handler = GuildAvailable;
			handler?.Invoke(this, g);
		}

		internal void GuildUnavailableEvent(Guild g)
		{
			var handler = GuildUnavailable;
			handler?.Invoke(this, g);
		}
		
		internal void GuildDeleteEvent(Guild g)
		{
			var handler = GuildDelete;
			handler?.Invoke(this, g);
		}
	}
}