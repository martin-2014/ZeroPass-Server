using Dapper;
using System.Data;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    internal partial class DomainRepository : IDomainRepository
    {
        readonly IDbConnection Connection;

        public DomainRepository(IUnitOfWork unitOfWork) => Connection = unitOfWork.Connection;

        public Task<int> Insert(DomainEntity entity)
        {
            var sql = "insert into t_domain" +
                "(domain_type, domain_name, company, create_time)" +
                "values " +
                "(@DomainType, @DomainName, @Company, @CreateTime);" +
                "select last_insert_id();";
            return Connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task InsertDomainInfo(DomainInfoEntity entity)
        {
            var sql = "insert into t_domain_info" +
                "(domain_id, contact_phone, contact_person, number_of_employees, country, timezone, logo)" +
                "values " +
                "(@DomainId, @ContactPhone, @ContactPerson, @NumberOfEmployees, @Country, @Timezone, @Logo);" +
                "select last_insert_id();";
            await Connection.ExecuteAsync(sql, entity);
        }

        public Task<DomainEntity> GetDomainByName(string domainName)
        {
            var sql = $@"select id, domain_type, domain_name, company, create_time
                from t_domain 
                where domain_name = @DomainName";
            return Connection.QueryFirstOrDefaultAsync<DomainEntity>(sql, new { DomainName = domainName });
        }
        
        public async Task UpdateDomainLogo(DomainInfoEntity entity)
        {
            var sql = "update t_domain_info " +
                      "set logo = @Logo, " +
                      "update_time = @UpdateTime, " +
                      "updated_by = @UpdatedBy " +
                      "where domain_id = @DomainId";
            await Connection.ExecuteAsync(sql, entity);
        }
    }
}
