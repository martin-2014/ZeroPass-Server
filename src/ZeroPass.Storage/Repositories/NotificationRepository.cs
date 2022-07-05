using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public class NotificationRepository : INotificationRepository
    {
        readonly IDbConnection Connection;

        public NotificationRepository(IUnitOfWork unitOfWork)
        {
            Connection = unitOfWork.Connection;
        }
        
        public async Task Insert<T>(IEnumerable<NotificationEntity<T>> values)
        {
            var sql = @"insert into t_notification (user_id, type, status, body, create_time)
                values (@UserId, @Type, @Status, @Body, @CreateTime)";
            await Connection.ExecuteAsync(sql, values);
        }
        
        public async Task SetStatus(int userId, IEnumerable<int> ids, int newStatus) 
        {
            var sql = @"update t_notification 
                set status = @Status, update_time = @UpdateTime
                where id in @Ids and user_id = @UserId";
            
            await Connection.ExecuteAsync(sql, new {UserId = userId, Ids = ids, Status = newStatus, UpdateTime = DateTime.UtcNow });
        }

        public async Task Process(int userId, IEnumerable<int> ids, int newStatus, JsonElement result)
        {
            var sql = @"update t_notification 
                set status = @Status, update_time = @UpdateTime, result = @Result
                where id in @Ids and user_id = @UserId";
            
            await Connection.ExecuteAsync(
                sql,
                new
                {
                    UserId = userId,
                    Ids = ids,
                    Status = newStatus,
                    Result = result,
                    UpdateTime = DateTime.UtcNow
                });
        }
        
        public Task<IEnumerable<NotificationEntity<T>>> ListActive<T>(int userId)
        {
            var sql = @$"select id, user_id, type, status, body, create_time, update_time
                from t_notification 
                where user_id = @UserId and status = 1
                order by create_time desc";
            
            return Connection.QueryAsync<NotificationEntity<T>>(sql, new { UserId = userId });
        }
    }
}