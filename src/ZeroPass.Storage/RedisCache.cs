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

        public async Task<byte[]> GetBytes(string key)
            => await DistributedCache.GetAsync(key);

        public Task Clear(string key)
            => DistributedCache.RemoveAsync(key);

        public async Task SetBytesWithAbsoluteExpiration(string key, byte[] value, int expireInMsec)
        {
            var options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(expireInMsec));
            await DistributedCache.SetAsync(key, value, options);
        }
    }
}
