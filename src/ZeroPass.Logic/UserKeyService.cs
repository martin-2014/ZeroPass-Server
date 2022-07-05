using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public partial class UserKeyService : IUserKeyService
    {
        const int ExpireDomainOwner = 60 * 60 * 1000;
        
        readonly IUnitOfWorkFactory UnitOfWorkFactory;
        readonly IUserKeyInternalService UserKeyInternalService;
        readonly ICache Cache;
        readonly ICacheKeyGenerator CacheKeyGenerator;

        public UserKeyService(
            IUnitOfWorkFactory unitOfWorkFactory, 
            IUserKeyInternalService userKeyInternalService, 
            ICache cache, ICacheKeyGenerator cacheKeyGenerator)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            UserKeyInternalService = userKeyInternalService;
            Cache = cache;
            CacheKeyGenerator = cacheKeyGenerator;
        }

        public async Task<UserPublicKeyModel> ExchangePublicKey(UserExchangePublicKeyModel model)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await UserKeyInternalService.ExchangePublicKey(unitOfWork, model);
        }

        public async Task<string> Authenticate(AuthenticateModel model)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await UserKeyInternalService.Authenticate(unitOfWork, model);
        }
        
        public async Task<string> GetDataKey(IDomainActor actor, UserKeyRequestModel model)
        {
            var domainOwner = await GetDomainOwnerByDomainId(actor.DomainId);
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await UserKeyInternalService.GetDataKey(unitOfWork, actor.UserId, domainOwner.UserId, model);
        }
        
        async Task<DomainUserEntity> GetDomainOwnerByDomainId(int domainId)
        {
            var key = CacheKeyGenerator.GenerateDomainOwnerByDomainId(domainId);
            var bytes = await Cache.GetBytes(key);

            using var unitOfWork = await UnitOfWorkFactory.CreateRead(domainId, DomainDataType.DomainUser);
            var entity = bytes != null ?
                bytes.ToEntity<DomainUserEntity>() :
                await unitOfWork.DomainUsers.GetDomainOwnerByDomainId(domainId);

            if (entity != null) await Cache.SetBytesWithAbsoluteExpiration(key, entity.ToByteArray(), ExpireDomainOwner);
            return entity;
        }
        
        public Task<bool> ActiveSession(int userId)
        {
            return UserKeyInternalService.ActiveSession(userId);
        }
    }
}