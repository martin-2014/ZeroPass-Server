using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using ZeroPass.Model.Configuration;

namespace ZeroPass.Storage
{
    internal class RedisCache : ICache
    {
        IDistributedCache DistributedCache;
        IConfiguration Configuration;

        public RedisCache(IDistributedCache distributedCache, IConfiguration configuration)
        {
            DistributedCache = distributedCache;
            Configuration = configuration;
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
        
        public async Task<IEnumerable<string>> GetKeys(string pattern)
        {
            var options = ConfigurationOptions.Parse(Configuration.GetValue("RedisConnectionString"));
            using var connection = await ConnectionMultiplexer.ConnectAsync(options);
            var endpoint = connection.GetEndPoints().First();
            var keys = connection.GetServer(endpoint).KeysAsync(pattern: pattern);
            var result = new List<string>();
            await foreach(var key in keys)
            {
                result.Add(key.ToString());
            }
            return result;
        }
    }
}
