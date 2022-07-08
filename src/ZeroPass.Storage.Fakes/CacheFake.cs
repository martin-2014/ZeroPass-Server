using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZeroPass.Storage.Fakes
{
    public class CacheFake : ICache
    {
        public readonly Dictionary<string, object> Values = new Dictionary<string, object>();

        public Task<string> Get(string key)
        {
            var value = Values.TryGetValue(key, out var v) ? v : null;
            return Task.FromResult(value?.ToString());
        }

        public Task<byte[]> GetBytes(string key)
        {
            var value = Values.TryGetValue(key, out var v) ? v : null;
            return Task.FromResult((byte[])value);
        }

        public Task SetBytesWithAbsoluteExpiration(string key, byte[] value, int expireInMsec)
        {
            Values[key] = value;
            return Task.CompletedTask;
        }

        public Task Clear(string key)
        {
            Values.Remove(key);
            return Task.CompletedTask;
        }

        public Task SetBytesWithSlidingExpiration(string key, byte[] value, int expireInMsec)
        {
            Values[key] = value;
            return Task.CompletedTask;
        }
        
        public Task SetWithAbsoluteExpiration(string key, string value, int expireMsec)
        {
            Values[key] = value;
            return Task.CompletedTask;
        }
        
        public Task SlidingExpiration(string key) => Task.CompletedTask;
        
        public Task<IEnumerable<string>> GetKeys(string pattern)
        {
            var regexPattern = pattern.Replace("$", "\\$").Replace(".", "\\.").Replace("*", ".*");
            var keys = Values.Keys.Where(k => Regex.IsMatch(k, regexPattern));
            return Task.FromResult(keys);
        }

        public Task SetBytes(string key, byte[] value)
        {
            Values[key] = value;
            return Task.CompletedTask;
        }
    }
}
