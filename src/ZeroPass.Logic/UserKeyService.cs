using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;

namespace ZeroPass.Service
{
    public partial class UserKeyService : IUserKeyService
    {
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

        public async Task<string> Authenticate(AuthenticateModel model, string deviceId)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await UserKeyInternalService.Authenticate(unitOfWork, model, deviceId);
        }

        public async Task<string> GetDataKey(IDomainActor actor, UserKeyRequestModel model, string deviceId)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await UserKeyInternalService.GetDataKey(unitOfWork, actor.UserId, actor.UserId, model, deviceId);
        }

        public Task<bool> ActiveSession(int userId, string deviceId)
        {
            return UserKeyInternalService.ActiveSession(userId, deviceId);
        }
    }
}