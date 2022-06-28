using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using ZeroPass.Model;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Service.Properties;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public class ActivationService : IActivationService
    {
        readonly ICacheKeyGenerator CacheKeyGenerator;
        readonly ICache Cache;
        readonly IMapper Mapper;
        readonly IEmailService EmailService;
        readonly IUserKeyInternalService UserKeyInternalService;
        
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory;

        public ActivationService(
            ICacheKeyGenerator cacheKeyGenerator,
            ICache cache,
            IMapper mapper,
            IUnitOfWorkFactory unitOfWorkFactory,
            IEmailService emailService,
            IUserKeyInternalService userKeyInternalService)
        {
            CacheKeyGenerator = cacheKeyGenerator;
            Cache = cache;
            Mapper = mapper;
            UnitOfWorkFactory = unitOfWorkFactory;
            EmailService = emailService;
            UserKeyInternalService = userKeyInternalService;
        }

        public async Task<CodeVerifyResultModel> CodeVerify(CodeVerifyModel model)
        {
            var entity = await GetRegistrationEntityFromCache(model.Email, model.Code);
            if (entity == null) return null;

            return Mapper.Map<CodeVerifyResultModel>(entity);
        }

        public async Task<ActivatedAccountModel> ActivateAccount(ActivateAccountModel model)
        {
            var entity = await GetRegistrationEntityFromCache(model.Email, model.Code);
            if (entity == null) return null;

            var activated = await Activate(entity, model.UserKey);

            if (activated != null) await RemoveRegistrationEntityFromCache(model.Email);

            return activated;
        }

        protected virtual Task<ActivatedAccountModel> Activate(RegistrationEntity registrationEntity, UserKeyCreateModel userKeyModel)
            => ActivateUser(registrationEntity, userKeyModel);

        async Task<RegistrationEntity> GetRegistrationEntityFromCache(string email, string code)
        {
            var key = CacheKeyGenerator.GenerateActivationKey(email);
            var bytes = await Cache.GetBytes(key);
            if (bytes == null) return null;

            var entity = bytes.ToEntity<RegistrationEntity>();
            if (entity.Code != code) return null;

            return entity;
        }

        async Task RemoveRegistrationEntityFromCache(string email)
            => await Cache.Clear(CacheKeyGenerator.GenerateActivationKey(email));

        protected async Task<ActivatedAccountModel> ActivateUser(RegistrationEntity registrationEntity, UserKeyCreateModel userKeyModel)
        {
            var userRegister = (UserRegisterModel)registrationEntity.Raw;
            var domainRegister = CreatePersonalDomain(userRegister.Email);

            using var unitOfWork = await UnitOfWorkFactory.CreateWrite();
            await unitOfWork.BeginTrans();
            try
            {
                var domain = CreateDomainEntity(domainRegister, DomainType.Personal);
                var domainId = await unitOfWork.Domains.Insert(domain);

                var domainInfo = CreateDomainInfoEntity(domainId, domainRegister);
                await unitOfWork.Domains.InsertDomainInfo(domainInfo);

                var user = CreateUserEntity(userRegister);
                var userId = await unitOfWork.Users.Insert(user);

                var userProfile = CreateUserProfileEntity(userId, userRegister.Timezone);
                await unitOfWork.UserProfiles.Insert(userProfile);

                var domainUser = CreateDomainUserEntity(domainId, userId);
                await unitOfWork.DomainUsers.Upsert(domainUser);

                userKeyModel.UserId = userId;
                await UserKeyInternalService.CreateUserKey(unitOfWork, userKeyModel);

                await unitOfWork.CommitTrans();

                await EmailService.Send(
                    new List<string> { userRegister.Email },
                    Resources.Email_Register_Success_Subject,
                    string.Format(Resources.Email_Register_Succeed, user.Email, string.Empty));
                return new ActivatedAccountModel
                {
                    UserId = userId,
                    IsBusiness = false
                };
            }
            catch
            {
                await unitOfWork.RollbackTrans();
                throw;
            }
        }

        protected DomainInfoRegisterModel CreatePersonalDomain(string email)
            => new DomainInfoRegisterModel
            {
                DomainName = $"Personal-{Guid.NewGuid()}",
                Company = $"Personal-{Guid.NewGuid()}",
                ContactPerson = email,
            };

        protected DomainEntity CreateDomainEntity(DomainInfoRegisterModel value, DomainType domainType)
            => new DomainEntity
            {
                Company = value.Company,
                CreateTime = DateTime.UtcNow,
                DomainName = value.DomainName,
                DomainType = domainType,
            };

        protected DomainInfoEntity CreateDomainInfoEntity(int domainId, DomainInfoRegisterModel value)
            => new DomainInfoEntity
            {
                DomainId = domainId,
                ContactPerson = value.ContactPerson,
                ContactPhone = value.ContactPhone,
                Country = value.Country,
                NumberOfEmployees = value.NumberOfEmployees,
                Timezone = value.Timezone,
            };

        protected UserEntity CreateUserEntity(UserRegisterModel value)
            => new UserEntity
            {
                UserType = (int)value.AccountType,
                Email = value.Email,
                UserName = new MailAddress(value.Email).User,
            };


        protected UserProfileEntity CreateUserProfileEntity(int userId, string timezone)
            => new UserProfileEntity
            {
                UserId = userId,
                Timezone = timezone
            };

        protected DomainUserEntity CreateDomainUserEntity(int domainId, int userId)
            => new DomainUserEntity
            {
                DomainId = domainId,
                UserId = userId,
                IsOwner = true,
                IsAdmin = true,
                Status = UserStatus.Active,
            };
    }
}
