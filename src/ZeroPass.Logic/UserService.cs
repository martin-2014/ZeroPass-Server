using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Model;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Service.Properties;
using ZeroPass.Storage;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Service
{
    public partial class UserService : IUserService
    {
        const int CodeExpireDefaultInMsec = 1800 * 1000;

        readonly IUnitOfWorkFactory UnitOfWorkFactory;
        readonly IRandom Generator;
        readonly ICacheKeyGenerator CacheKeyGenerator;
        readonly ICache Cache;
        readonly IEmailService EmailService;

        public UserService(
            IUnitOfWorkFactory factory, 
            IRandom generator, 
            ICacheKeyGenerator cacheKeyGenerator, 
            ICache cache, 
            IEmailService emailService)
        {
            UnitOfWorkFactory = factory;
            Generator = generator;
            CacheKeyGenerator = cacheKeyGenerator;
            Cache = cache;
            EmailService = emailService;
        }

        public async Task<bool> ExistsUserByEmail(string email)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            return await unitOfWork.Users.GetByEmail(email) != null;
        }

        public async Task RegisterUser(UserRegisterModel user)
        {
            var code = Generator.GenerateVerificationCode();
            var key = CacheKeyGenerator.GenerateActivationKey(user.Email);

            await Cache.SetBytesWithAbsoluteExpiration(
                key,
                CreateRegistrationEntity(user, code).ToByteArray(),
                CodeExpireDefaultInMsec);

            await EmailService.Send(
                new List<string> { user.Email },
                string.Format(Resources.Email_Register_Code_Subject, code),
                string.Format(Resources.Email_Register_Code, code));
        }

        RegistrationEntity CreateRegistrationEntity(UserRegisterModel model, string code)
           => new RegistrationEntity
           {
               ActivateType = ActivateType.Personal,
               Code = code,
               Email = model.Email,
               Raw = model
           };
    }
}
