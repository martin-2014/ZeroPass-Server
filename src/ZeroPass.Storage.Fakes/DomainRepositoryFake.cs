﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
    public partial class DomainRepositoryFake : RepositoryFake<DomainEntity>, IDomainRepository
    {
        readonly FakeDatabase Database;

        public readonly List<DomainEntity> DomainEntities;
        public readonly List<DomainInfoEntity> DomainInfoEntities;

        public DomainRepositoryFake(FakeDatabase database) : base(database.Domains)
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

        public Task<DomainEntity> GetDomainByName(string domainName)
        {
            var item = DomainEntities.FirstOrDefault(d => d.DomainName == domainName);
            return Task.FromResult(item);
        }
        
        public Task UpdateDomainLogo(DomainInfoEntity entity)
        {
            var item = DomainInfoEntities.FirstOrDefault(d => d.DomainId == entity.DomainId);
            item.Logo = entity.Logo;
            return Task.CompletedTask;
        }
        
        public Task<IEnumerable<DomainEntity>> GetDomainByIds(IEnumerable<int> ids)
        {
            var domains = DomainEntities.Where(d => ids.Contains(d.Id));
            return Task.FromResult(domains);
        }
        
        public Task<DomainEntity> GetDomainById(int id)
        {
            var entity = DomainEntities.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(entity);
        }
    }
}
