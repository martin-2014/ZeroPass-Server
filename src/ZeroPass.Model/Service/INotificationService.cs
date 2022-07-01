using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroPass.Model.Models;

namespace ZeroPass.Model.Service
{
    public partial interface INotificationService
    {
        Task<IEnumerable<NotificationModel<T>>> ListActive<T>(int userId);
        
        Task Process(int userId, NotificationActionResultModel value);
        
        Task Clear(int userId, IEnumerable<int> notificationIds);
    }
}