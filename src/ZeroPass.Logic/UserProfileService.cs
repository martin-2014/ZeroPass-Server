using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ZeroPass.Model.Models;
using ZeroPass.Model.Models.UserProfiles;
using ZeroPass.Model.Service;
using ZeroPass.Storage;

namespace ZeroPass.Service
{
    public partial class UserProfileService : IUserProfileService
    {
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
        protected readonly IMapper Mapper;

        public UserProfileService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Mapper = mapper;
        }

        public async Task<UserProfileModel> GetUserProfile(IActor actor)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateRead();
            var profileModel = Mapper.Map<UserProfileModel>(
                await unitOfWork.UserProfiles.GetProfile(actor.UserId));

            var domains = await unitOfWork.DomainUsers.GetDomainDetailsByUserId(actor.UserId);
            var domainOfUsers = Mapper.Map<IEnumerable<DomainOfUserModel>>(
                domains.Where(d => d.Status == Storage.Entities.UserStatus.Active));

            profileModel.Domains = domainOfUsers;
            return profileModel;
        }
    }
}