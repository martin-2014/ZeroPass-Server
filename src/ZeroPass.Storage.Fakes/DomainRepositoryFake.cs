using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public class DomainRepositoryFake : RepositoryFake<DomainEntity>, IDomainRepository
    {
        readonly FakeDatabase Database;

        public readonly List<DomainEntity> DomainEntities;
        public readonly List<DomainInfoEntity> DomainInfoEntities;

        public DomainRepositoryFake(FakeDatabase database)
        {
            Database = database;
            DomainEntities = database.Domains;
            DomainInfoEntities = database.DomainInfos;
        }

        public Task<int> Insert(DomainEntity entity)
        {
            DomainEntities.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public Task InsertDomainInfo(DomainInfoEntity entity)
        {
            DomainInfoEntities.Add(entity);
            return Task.CompletedTask;
        }
    }
}
