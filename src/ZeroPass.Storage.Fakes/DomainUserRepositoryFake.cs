using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class DomainUserRepositoryFake : IDomainUserRepository
    {
        public readonly List<DomainUserEntity> DomainUserEntities;

        public DomainUserRepositoryFake(FakeDatabase database)
        {
            DomainUserEntities = database.DomainUsers;
        }

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
    }
}
