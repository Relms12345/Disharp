using System;

namespace Disharp.Rest
{
	public class Cdn
	{
		public Cdn(string strBase)
		{
			Base = strBase;
		}

		private string Base { get; }

		public string AppAsset(string clientId, string assetHash, ImageUrlOptions options)
		{
			return MakeUrl($"/app-assets/{clientId}/{assetHash}", options);
		}

		public string AppIcon(string clientId, string iconHash, ImageUrlOptions options)
		{
			return MakeUrl($"/app-icons/{clientId}/{iconHash}", options);
		}

		public string DefaultAvatar(int discriminator, ImageUrlOptions options)
		{
			return MakeUrl($"/embed/avatars/{discriminator}", options);
		}

		public string DiscoverySplash(string guildId, string splashHash, ImageUrlOptions options)
		{
			return MakeUrl($"/discovery-splashes/{guildId}/{splashHash}", options);
		}

		public string Emoji(string emojiId, ImageUrlOptions options)
		{
			return MakeUrl($"/emojis/${emojiId}", options);
		}

		public string GroupDmIcon(string channelId, string iconHash, ImageUrlOptions options)
		{
			return MakeUrl($"/channel-icons/{channelId}/{iconHash}", options);
		}

		public string GuildBanner(string guildId, string bannerHash, ImageUrlOptions options)
		{
			return MakeUrl($"/banners/{guildId}/{bannerHash}", options);
		}

		public string GuildIcon(string guildId, string iconHash, ImageUrlOptions options)
		{
			return MakeUrl($"/icons/{guildId}/{iconHash}", options);
		}

		public string Splash(string guildId, string splashHash, ImageUrlOptions options)
		{
			return MakeUrl($"/splashes/{guildId}/{splashHash}", options);
		}

		public string TeamIcon(string teamId, string iconHash, ImageUrlOptions options)
		{
			return MakeUrl($"/team-icons/{teamId}/{iconHash}", options);
		}

		public string UserAvatar(string userId, string avatarHash, ImageUrlOptions options)
		{
			if (options.Dynamic) options.Extension = avatarHash.StartsWith("a_") ? "gif" : options.Extension;
			return MakeUrl($"/avatars/{userId}/{avatarHash}", options);
		}

		private string MakeUrl(string endpoint, ImageUrlOptions options)
		{
			var extension = Convert.ToString(options.Extension)?.ToLower();

			var url = new Uri($"{Base}{endpoint}.{extension}?size={options.Size}");

			return url.ToString();
		}
	}
}