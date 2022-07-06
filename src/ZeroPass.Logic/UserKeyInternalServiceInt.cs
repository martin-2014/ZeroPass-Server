using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ZeroPass.Model;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;

namespace ZeroPass.Service
{
    internal partial class UserKeyInternalService
    {
        public UserKeyInternalService(IServiceProvider provider)
        {
            SessionFactory = provider.GetService<ISessionFactory>();
            CacheKeyGenerator = provider.GetService<ICacheKeyGenerator>();
            Mapper = provider.GetService<IMapper>();
            Cache = provider.GetService<ICache>();
            CryptoService = provider.GetService<ICryptoService>();
        }
        
        public async Task<string> GetDataKey(IUnitOfWork unitOfWork, int actorId, int assignerId, UserKeyRequestModel model, string deviceId)
        {
            var userSession = await GetUserKeySession(actorId, deviceId);
            Validatable(userSession, model);
            var userKey = await GetUserKeyById(unitOfWork, actorId);
            var actorUserKey = await GetUserKeyById(unitOfWork, actorId);
            var dataKeyJson = JsonConvert.SerializeObject(new DataKeyModel
            {
                AssignerId = actorId,
                PublicKey = userKey.PublicDataKey,
                SelfPrivateKey = actorUserKey.PrivateDataKey,
            });
            return CryptoService.EncryptText(dataKeyJson, userSession.CommunicateKey);
        }
    }
}