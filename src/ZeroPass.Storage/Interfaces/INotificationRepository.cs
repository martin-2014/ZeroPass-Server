using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage
{
    public interface INotificationRepository
    {
        Task Insert<T>(IEnumerable<NotificationEntity<T>> values);
        
        Task SetStatus(int userId, IEnumerable<int> ids, int newStatus);

        Task Process(int userId, IEnumerable<int> ids, int newStatus, JsonElement result);

        Task<IEnumerable<NotificationEntity<T>>> ListActive<T>(int userId);
    }
}