using AutoMapper;
using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public interface IUserKeyInternalService
    {
        Task<bool> CreateUserKey(IUnitOfWork unitOfWork, UserKeyCreateModel model);

        Task<UserKeyEntity> GetUserKeyById(IUnitOfWork unitOfWork, int userId);
    }

    internal class UserKeyInternalService : IUserKeyInternalService
    {
        const int ExpireUserKey = 24 * 60 * 60 * 1000;

        readonly IMapper Mapper;
        readonly ICacheKeyGenerator CacheKeyGenerator;
        readonly ICache Cache;

        public UserKeyInternalService(
            IMapper mapper,
            ICacheKeyGenerator cacheKeyGenerator,
            ICache cache)
        {
            Mapper = mapper;
            CacheKeyGenerator = cacheKeyGenerator;
            Cache = cache;
        }

        public async Task<bool> CreateUserKey(IUnitOfWork unitOfWork, UserKeyCreateModel model)
        {
            if (await GetUserKeyById(unitOfWork, model.UserId) != null) 
                return false;

            var entity = Mapper.Map<UserKeyEntity>(model);
            await unitOfWork.UserKeys.Insert(entity);

            return true;
        }

        public async Task<UserKeyEntity> GetUserKeyById(IUnitOfWork unitOfWork, int userId)
        {
            var key = CacheKeyGenerator.GenerateUserKeyById(userId);
            var bytes = await Cache.GetBytes(key);

            var entity = bytes != null ?
                bytes.ToEntity<UserKeyEntity>() :
                await unitOfWork.UserKeys.GetByUserId(userId);

            if (entity != null) 
                await Cache.SetBytesWithAbsoluteExpiration(key, entity.ToByteArray(), ExpireUserKey);

            return entity;
        }
    }
}
