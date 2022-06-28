using Dapper;
using System.Data;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    internal class DomainUserRepository : IDomainUserRepository
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
    }
}
