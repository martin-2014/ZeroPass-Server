using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public partial class DomainUserRepositoryFake : IDomainUserRepository
    {
        public List<DomainUserEntity> DomainUserEntities;
        public List<DomainEntity> DomainEntities;
        public List<DomainInfoEntity> DomainInfoEntities;
        public List<UserEntity> UserEntities;

        public DomainUserRepositoryFake(FakeDatabase database) => Initialize(database);

        public Task Upsert(DomainUserEntity entity)
        {
            var user = DomainUserEntities.FirstOrDefault(u => u.DomainId == entity.DomainId && u.UserId == entity.UserId);
            if (user == null)
            {
                DomainUserEntities.Add(entity);
            }
            else
            {
                user.Status = entity.Status;
                user.UpdatedBy = entity.UpdatedBy;
                user.IsOwner = entity.IsOwner;
                user.IsAdmin = entity.IsAdmin;
                user.UpdateTime = entity.UpdateTime;
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<UserDomainView>> GetDomainsByUserId(int userId)
        {
            var views = from domainUser in DomainUserEntities
                join domain in DomainEntities on domainUser.DomainId equals domain.Id
                where domainUser.UserId == userId
                select new UserDomainView
                {
                    Domain = domain,
                    DomainUser = domainUser
                };
            return Task.FromResult(views);
        }
        
        public Task<DomainUserEntity> GetDomainOwnerByDomainId(int domainId)
        {
            var entity = DomainUserEntities.FirstOrDefault(d => d.DomainId == domainId && d.IsOwner);
            return Task.FromResult(entity);
        }
        
        public Task<IEnumerable<DomainUserDetailView>> GetDomainDetailsByUserId(int userId)
        {
            var views = from domainUser in DomainUserEntities
                join domain in DomainEntities on domainUser.DomainId equals domain.Id
                join domainInfo in DomainInfoEntities on domain.Id equals domainInfo.DomainId
                where domainUser.UserId == userId
                select new DomainUserDetailView
                {
                    DomainId = domain.Id,
                    Company = domain.Company,
                    DomainName = domain.DomainName,
                    DomainType = domain.DomainType,
                    IsAdmin = domainUser.IsAdmin,
                    IsOwner = domainUser.IsOwner,
                    Logo = domainInfo.Logo,
                    Status = domainUser.Status,
                    Timezone = domainInfo.Timezone,
                };
            return Task.FromResult(views);
        }
    }
}
