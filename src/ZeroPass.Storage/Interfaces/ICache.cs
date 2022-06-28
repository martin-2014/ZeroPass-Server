using System.Threading.Tasks;

namespace ZeroPass.Storage
{
    public interface ICache
    {
        Task SetBytesWithAbsoluteExpiration(string key, byte[] value, int expireInMsec);

        Task<byte[]> GetBytes(string key);

        Task Clear(string key);
    }
}
