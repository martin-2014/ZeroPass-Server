using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZeroPass.Storage.Fakes
{
    public class CacheFake : ICache
    {
        public readonly Dictionary<string, object> Values = new Dictionary<string, object>();

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
    }
}
