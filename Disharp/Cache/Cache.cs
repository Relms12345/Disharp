using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Disharp.Client;
using Microsoft.Extensions.Caching.Memory;

namespace Disharp.Cache
{
	public class Cache<T>
	{
		private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions
		{
			CompactionPercentage = 1,
			ExpirationScanFrequency = TimeSpan.FromMinutes(1)
		});

		private readonly DisharpClient _client;

		private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks =
			new ConcurrentDictionary<object, SemaphoreSlim>();

		public Cache(DisharpClient client)
		{
			_client = client;
		}

		public async Task<T> GetAsync(string key)
		{
			if (_cache.TryGetValue(key, out T cacheEntry)) return cacheEntry;
			var mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

			await mylock.WaitAsync();
			try
			{
				if (!_cache.TryGetValue(key, out cacheEntry)) cacheEntry = default;
			}
			finally
			{
				mylock.Release();
			}

			return cacheEntry;
		}

		public async Task<T> GetOrCreateAsync(string key, Func<Task<T>> createItem)
		{
			if (_cache.TryGetValue(key, out T cacheEntry)) return cacheEntry;
			var mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

			await mylock.WaitAsync();
			try
			{
				if (!_cache.TryGetValue(key, out cacheEntry))
				{
					// Key not in cache, so get data.
					cacheEntry = await createItem();

					var cacheEntryOptions = new MemoryCacheEntryOptions()
						.SetSize(1)
						.SetPriority(CacheItemPriority.High)
						.SetSlidingExpiration(_client.ClientOptions.CacheOptions.CacheEntryReadExpiration)
						.SetAbsoluteExpiration(_client.ClientOptions.CacheOptions.CacheEntryAbsoluteExpiration);

					_cache.Set(key, cacheEntry, cacheEntryOptions);
				}
			}
			finally
			{
				mylock.Release();
			}

			return cacheEntry;
		}

		public async Task<T> UpdateAsync(string key, T updatedItem)
		{
			if (!_cache.TryGetValue(key, out T cacheEntry)) return default;
			var mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

			await mylock.WaitAsync();
			try
			{
				if (_cache.TryGetValue(key, out cacheEntry))
				{
					var cacheEntryOptions = new MemoryCacheEntryOptions()
						.SetSize(1)
						.SetPriority(CacheItemPriority.High)
						.SetSlidingExpiration(_client.ClientOptions.CacheOptions.CacheEntryReadExpiration)
						.SetAbsoluteExpiration(_client.ClientOptions.CacheOptions.CacheEntryAbsoluteExpiration);

					cacheEntry = _cache.Set(key, updatedItem, cacheEntryOptions);
				}
			}
			finally
			{
				mylock.Release();
			}

			return cacheEntry;
		}

		public async Task<bool> DeleteAsync(string key)
		{
			if (_cache.TryGetValue(key, out T _))
			{
				_cache.Remove(key);
				return true;
			}

			var mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

			await mylock.WaitAsync();
			try
			{
				if (!_cache.TryGetValue(key, out T _)) return false;
			}
			finally
			{
				mylock.Release();
			}

			return true;
		}
	}
}