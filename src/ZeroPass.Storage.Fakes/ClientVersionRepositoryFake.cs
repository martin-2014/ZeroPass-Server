using System;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities.ClientVersions;

namespace ZeroPass.Storage.Fakes
{
    public class ClientVersionRepositoryFake : IClientVersionRepository
    {
        readonly FakeDatabase Database;

        public ClientVersionRepositoryFake(FakeDatabase database) => Database = database;

        public Task<ClientMinVersionEntity> GetMinRequiredVersionByEdition(string edition) 
            => Task.FromResult(Database.ClientMinVersions.FirstOrDefault(v => v.Edition.Equals(edition, StringComparison.OrdinalIgnoreCase)));

        public Task SaveClientVersion(ClientVersionView version)
        {
            var user = Database.Users.First(u => u.Email.Equals(version.Email));
            var existVersion =
                Database.ClientVersions.FirstOrDefault(v => v.UserId == user.Id && v.DeviceId == version.DeviceId);
            if (existVersion == null)
            {
                existVersion = new ClientVersionEntity
                {
                    UserId = user.Id, Edition = version.Edition, DeviceId = version.DeviceId, Version = version.Version
                };
                
                Database.ClientVersions.Add(existVersion);
            }
            else
            {
                existVersion.Edition = version.Edition;
                existVersion.Version = version.Version;
            }
            
            return Task.CompletedTask;
        }
    }
}