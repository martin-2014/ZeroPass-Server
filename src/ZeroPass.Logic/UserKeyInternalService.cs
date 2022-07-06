using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZeroPass.Model;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public partial interface IUserKeyInternalService
    {
        Task<bool> CreateUserKey(IUnitOfWork unitOfWork, UserKeyCreateModel model);

        Task<UserKeyEntity> GetUserKeyById(IUnitOfWork unitOfWork, int userId);
        
        Task<UserPublicKeyModel> ExchangePublicKey(IUnitOfWork unitOfWork, UserExchangePublicKeyModel model);
        
        Task<string> Authenticate(IUnitOfWork unitOfWork, AuthenticateModel model, string deviceId);
        
        Task<string> GetDataKey(IUnitOfWork unitOfWork, int actorId, int assignerId, UserKeyRequestModel model, string deviceId);

        Task<bool> ActiveSession(int userId, string deviceId);
    }

    internal partial class UserKeyInternalService : IUserKeyInternalService
    {
        const int ExpireUserKey = 24 * 60 * 60 * 1000;
        const int ExpireAuthentication = 60 * 1000;
        const int ExpireSession = 24 * 60 * 60 * 1000;
        
        readonly ISessionFactory SessionFactory;
        readonly ICacheKeyGenerator CacheKeyGenerator;
        readonly IMapper Mapper;
        readonly ICache Cache;
        readonly ICryptoService CryptoService;

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

        public async Task<UserPublicKeyModel> ExchangePublicKey(IUnitOfWork unitOfWork, UserExchangePublicKeyModel model)
        {
            var userKey = await GetUserKeyByEmail(unitOfWork, model.Email);
            if (userKey == null) return null;

            return await ExchangePublicKeyByVerifier(userKey.Verifier, userKey.Salt, model.PublicKey);
        }
        
        public async Task<string> Authenticate(IUnitOfWork unitOfWork, AuthenticateModel model, string deviceId)
        {
            try
            {
                var exchangeKey = await GetExchangeKeyFromCache(model.KeyId);
                if (exchangeKey == null) return null;

                var userKey = await GetUserKeyByEmail(unitOfWork, model.Request.ClientIdentifierProof.Email);
                if (userKey == null) return null;

                var session = SessionFactory.CreateSession(
                    exchangeKey.ServerPrivateKey,
                    exchangeKey.ClientPublicKey,
                    userKey.Salt,
                    model.Request.ClientIdentifierProof.Email,
                    userKey.Verifier,
                    model.Request.ClientIdentifierProof.IdentifierProof
                );

                if (session == null) return null;

                if (!VerifyRequestData(model.Request, session.Key)) return null;

                await CreateUserKeySession(
                    userKey,
                    exchangeKey,
                    model.Request.ClientIdentifierProof.Email,
                    session.Key,
                    model.Request.Raw,
                    deviceId);

                return session.Proof;
            }
            finally
            {
                await RemoveExchangeKeyFromCache(model.KeyId);
            }
        }
        
        public async Task<bool> ActiveSession(int userId, string deviceId)
        {
            var session = await GetUserKeySession(userId, deviceId);
            return session != null;
        }

        void Validatable(UserKeySessionModel userSession, UserKeyRequestModel model)
        {
            if (userSession == null ||
                !VerifySession(userSession, model.ClientIdentifierProof.IdentifierProof) ||
                !VerifyRequestData(model, userSession.CommunicateKey))
            {
                throw new UnauthorizedAccessException();
            }
        }
        
        bool VerifySession(UserKeySessionModel userSession, string identifierProof)
            => SessionFactory.CreateSession(
                userSession.ServerPrivateKey,
                userSession.ClientPublicKey,
                userSession.Salt,
                userSession.Email,
                userSession.Verifier,
                identifierProof) != null;
        
        async Task<UserKeySessionModel> GetUserKeySession(int userId, string deviceId)
        {
            var cacheKey = CacheKeyGenerator.GenerateUserKeySession(userId, deviceId);
            var bytes = await Cache.GetBytes(cacheKey);

            if (bytes != null)
            {
                await Cache.SlidingExpiration(cacheKey);
                return bytes.ToEntity<UserKeySessionModel>();
            }
            else
            {
                return null;
            }
        }

        Task RemoveExchangeKeyFromCache(string keyId)
            => Cache.Clear(CacheKeyGenerator.GenerateExchangeKey(keyId));
        
        async Task<UserKeyExchangeModel> GetExchangeKeyFromCache(string keyId)
        {
            var cacheKey = CacheKeyGenerator.GenerateExchangeKey(keyId);
            var bytes = await Cache.GetBytes(cacheKey);
            return bytes?.ToEntity<UserKeyExchangeModel>();
        }
        
        async Task<UserKeyEntity> GetUserKeyByEmail(IUnitOfWork unitOfWork, string email)
        {
            var key = CacheKeyGenerator.GenerateUserKeyByEmail(email);
            var bytes = await Cache.GetBytes(key);

            var entity = bytes != null ?
                bytes.ToEntity<UserKeyEntity>() :
                await unitOfWork.UserKeys.GetByEmail(email);

            if (entity != null) await Cache.SetBytesWithAbsoluteExpiration(key, entity.ToByteArray(), ExpireUserKey);
            return entity;
        }
        
        async Task<UserPublicKeyModel> ExchangePublicKeyByVerifier(string verifier, string salt, string clientPublicKey)
        {
            var exchangeKey = await CreateExchangeKey(verifier, clientPublicKey);
            return new UserPublicKeyModel
            {
                ExchangeKeyId = exchangeKey.KeyId,
                PublicKey = exchangeKey.ServerPublicKey,
                AdditionalData = salt
            };
        }
        
        async Task<UserKeyExchangeModel> CreateExchangeKey(string verifier, string clientPublicKey)
        {
            var srpEphemeral = SessionFactory.CreateEphemeralModel(verifier);

            var session = new UserKeyExchangeModel
            {
                KeyId = Guid.NewGuid().ToString("N"),
                ServerPrivateKey = srpEphemeral.Secret,
                ServerPublicKey = srpEphemeral.Public,
                ClientPublicKey = clientPublicKey
            };

            var cacheKey = CacheKeyGenerator.GenerateExchangeKey(session.KeyId);
            await Cache.SetBytesWithAbsoluteExpiration(cacheKey, session.ToByteArray(), ExpireAuthentication);

            return session;
        }
        
        async Task CreateUserKeySession(
            UserKeyEntity userKey,
            UserKeyExchangeModel exchangeKey,
            string email,
            string key,
            string payload,
            string deviceId)
        {
            var session = new UserKeySessionModel
            {
                Id = userKey.UserId,
                Email = email,
                Salt = userKey.Salt,
                Verifier = userKey.Verifier,
                ServerPrivateKey = exchangeKey.ServerPrivateKey,
                ClientPublicKey = exchangeKey.ClientPublicKey,
                CommunicateKey = key,
                MasterKey = payload
            };

            var cacheKey = CacheKeyGenerator.GenerateUserKeySession(session.Id, deviceId);
            await Cache.SetBytesWithSlidingExpiration(cacheKey, session.ToByteArray(), ExpireSession);
        }
        
        static bool VerifyRequestData(UserKeyRequestModel clientRequest, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey)) return false;

            var plainText = JsonConvert.SerializeObject(clientRequest.ClientIdentifierProof) + clientRequest.Raw;
            var cryptoKey = Encoding.UTF8.GetBytes(secretKey);
            using var hmac = new HMACSHA256(cryptoKey);
            var signatureBuffer = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainText.ToLower()));

            return signatureBuffer.SequenceEqual(Convert.FromBase64String(clientRequest.Signature));
        }
    }
}
