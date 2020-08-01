using System;

namespace Disharp.Cache
{
	public class DisharpCacheOptions
	{
		public TimeSpan CacheEntryReadExpiration { get; set; } = TimeSpan.FromHours(1);
		public TimeSpan CacheEntryAbsoluteExpiration { get; set; } = TimeSpan.FromHours(12);
	}
}