using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disharp.WebSocket.Payloads.SpecificPayloadData
{
	public class GuildCreatePayload
	{
		[JsonProperty("joined_at")] public string JoinedAt { get; set; }

		[JsonProperty("roles")] public List<dynamic> Roles { get; set; }

		[JsonProperty("features")] public List<dynamic> Features { get; set; }

		[JsonProperty("presences")] public List<dynamic> Presences { get; set; }

		[JsonProperty("premium_tier")] public int PremiumTier { get; set; }

		[JsonProperty("system_channel_id")] public string SystemChannelId { get; set; }

		[JsonProperty("discovery_splash")] public string DiscoverySplash { get; set; }

		[JsonProperty("verification_level")] public int VerificationLevel { get; set; }

		[JsonProperty("voice_states")] public List<dynamic> VoiceStates { get; set; }

		[JsonProperty("explicit_content_filter")]
		public int ExplicitContentFilter { get; set; }

		[JsonProperty("owner_id")] public string OwnerId { get; set; }

		[JsonProperty("rules_channel_id")] public string RulesChannelId { get; set; }

		[JsonProperty("mfa_level")] public int MfaLevel { get; set; }

		[JsonProperty("large")] public bool Large { get; set; }

		[JsonProperty("unavailable")] public bool Unavailable { get; set; }

		[JsonProperty("preferred_locale")] public string PreferredLocale { get; set; }

		[JsonProperty("premium_subscription_count")]
		public int PremiumSubscriptionCount { get; set; }

		[JsonProperty("icon")] public string Icon { get; set; }

		[JsonProperty("members")] public List<dynamic> Members { get; set; }

		[JsonProperty("region")] public string Region { get; set; }

		[JsonProperty("splash")] public string Splash { get; set; }

		[JsonProperty("vanity_url_code")] public string VanityUrlCode { get; set; }

		[JsonProperty("name")] public string Name { get; set; }

		[JsonProperty("banner")] public string Banner { get; set; }

		[JsonProperty("member_count")] public int MemberCount { get; set; }

		[JsonProperty("public_updates_channel_id")]
		public string PublicUpdatesChannelId { get; set; }

		[JsonProperty("max_video_channel_users")]
		public int MaxVideoChannelUsers { get; set; }

		[JsonProperty("default_message_notifications")]
		public int DefaultMessageNotifications { get; set; }

		[JsonProperty("application_id")] public string ApplicationId { get; set; }

		[JsonProperty("afk_channel_id")] public string AfkChannelId { get; set; }

		[JsonProperty("system_channel_flags")] public string SystemChannelFlags { get; set; }

		[JsonProperty("emojis")] public List<dynamic> Emojis { get; set; }

		[JsonProperty("channels")] public List<dynamic> Channels { get; set; }

		[JsonProperty("description")] public string Description { get; set; }

		[JsonProperty("afk_timeout")] public int AfkTimeout { get; set; }

		[JsonProperty("id")] public string Id { get; set; }


		[JsonProperty("lazy")] internal bool Lazy { get; set; }
	}
}