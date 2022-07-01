using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface ICache
    {
        Task<string> Get(string key);

        Task<byte[]> GetBytes(string key);

        Task Clear(string key);
        
        Task SetBytesWithSlidingExpiration(string key, byte[] value, int expireInMsec);
        
        Task SetWithAbsoluteExpiration(string key, string value, int expireInMsec);
        
        Task SetBytesWithAbsoluteExpiration(string key, byte[] value, int expireInMsec);

        Task SlidingExpiration(string key);
    }
}
