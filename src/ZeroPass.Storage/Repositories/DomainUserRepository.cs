using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    internal partial class DomainUserRepository : IDomainUserRepository
    {
        readonly IDbConnection Connection;

        public DomainUserRepository(IUnitOfWork unitOfWork) => Connection = unitOfWork.Connection;

        public async Task Upsert(DomainUserEntity entity)
        {
            var sql = @"insert into t_domain_user (domain_id, user_id, is_owner, is_admin, status, created_by, create_time)
                        values (@DomainId, @UserId, @IsOwner, @IsAdmin, @Status, @CreatedBy, @CreateTime) 
                        on duplicate key update is_owner = @IsOwner, is_admin = @IsAdmin, status = @Status, 
                            updated_by = @UpdatedBy, update_time = @UpdateTime;";
            await Connection.ExecuteAsync(sql, entity);
        }

        public async Task<IEnumerable<UserDomainView>> GetDomainsByUserId(int userId)
        {
            var sql = $@"select d.id, d.domain_type, d.domain_name, d.company,
                d.id,
                du.domain_id, du.user_id, du.is_owner, du.is_admin, du.status
                from t_domain d 
                inner join t_domain_user du on d.id = du.domain_id
                where du.user_id = @UserId";
            return await Connection.QueryAsync<DomainEntity, DomainUserEntity, UserDomainView>(
                sql,
                param: new { UserId = userId },
                map: (domain, domainUser) => new UserDomainView
                {
                    Domain = domain,
                    DomainUser = domainUser
                });
        }
        
        public Task<DomainUserEntity> GetDomainOwnerByDomainId(int domainId)
        {
            var sql = $@"select domain_id, user_id, is_owner, is_admin, status  
                from t_domain_user  
                where domain_id = @DomainId and is_owner = true";
            
            return Connection.QueryFirstOrDefaultAsync<DomainUserEntity>(
                sql,
                new { DomainId = domainId });
        }
        
        public Task<IEnumerable<DomainUserDetailView>> GetDomainDetailsByUserId(int userId)
        {
            var sql = $@"select du.domain_id, d.domain_type, d.domain_name, d.company,
                    di.timezone, di.logo, du.is_owner, du.is_admin, du.status, di.setting
                from t_domain_user du inner join
                    t_domain d on du.domain_id = d.id inner join
                    t_domain_info di on di.domain_id = d.id
                where du.user_id = @UserId";
            
            return Connection.QueryAsync<DomainUserDetailView>(
                sql,
                new { UserId = userId });
        }
    }
}
