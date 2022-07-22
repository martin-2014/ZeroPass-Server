using System;
using System.Threading.Tasks;
using AutoMapper;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities.ClientVersions;

namespace ZeroPass.Service
{
    public class ClientVersionService : IClientVersionService
    {
        readonly IUnitOfWorkFactory UnitOfWorkFactory;
        protected readonly IMapper Mapper;

        public ClientVersionService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Mapper = mapper;
        }

        public async Task<(bool, string)> MeetMinimumVersion(ClientVersionModel version)
        {
            if (string.IsNullOrEmpty(version.Edition) || string.IsNullOrEmpty(version.Version))
            {
                return (false, string.Empty);
            }
            
            using var unitOfWork = await UnitOfWorkFactory.CreateReadonly();

            var minRequiredVersion = Mapper.Map<ClientMinVersionModel>(await unitOfWork.ClientVersions.GetMinRequiredVersionByEdition(version.Edition));
            if (minRequiredVersion == null)
            {
                return (false, string.Empty);
            }

            var minVersion = new Version(minRequiredVersion.MinVersion);
            var clientVersion = new Version(version.Version);

            return (clientVersion >= minVersion, minRequiredVersion.MinVersion);
        }

        public async Task SaveClientVersion(ClientVersionModel version)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateWrite();

            var entity = Mapper.Map<ClientVersionView>(version);

            await unitOfWork.ClientVersions.SaveClientVersion(entity);
        }
    }
}