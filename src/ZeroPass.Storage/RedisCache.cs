using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    internal class RedisCache : ICache
    {
        IDistributedCache DistributedCache;

        public RedisCache(IDistributedCache distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public async Task<string> Get(string key)
            => await DistributedCache.GetStringAsync(key);
        
        public async Task<byte[]> GetBytes(string key)
            => await DistributedCache.GetAsync(key);

        public Task Clear(string key)
            => DistributedCache.RemoveAsync(key);

        public async Task SetBytesWithSlidingExpiration(string key, byte[] value, int expireInMsec)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMilliseconds(expireInMsec));
            await DistributedCache.SetAsync(key, value, options);
        }

        public async Task SetBytesWithAbsoluteExpiration(string key, byte[] value, int expireInMsec)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expireInMsec));
            await DistributedCache.SetAsync(key, value, options);
        }
        
        public async Task SetWithAbsoluteExpiration(string key, string value, int expireInMsec)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expireInMsec));
            await DistributedCache.SetStringAsync(key, value, options);
        }
        
        public async Task SlidingExpiration(string key)
            => await DistributedCache.RefreshAsync(key);
    }
}
