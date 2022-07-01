using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroPass.Model.Models;
using ZeroPass.Model.Service;
using ZeroPass.Storage;

namespace ZeroPass.Service
{
    public class NotificationService : INotificationService
    {
        readonly IUnitOfWorkFactory UnitOfWorkFactory;

        public NotificationService(IUnitOfWorkFactory factory) => UnitOfWorkFactory = factory;

        public async Task Process(int userId, NotificationActionResultModel value)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateWrite();

            await unitOfWork.Notifications.Process(userId, value.Ids, (int)NotificationStatus.Processed, value.Result);
        }

        public async Task Clear(int userId, IEnumerable<int> notificationIds)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateWrite();

            await unitOfWork.Notifications.SetStatus(userId, notificationIds, (int)NotificationStatus.Processed);
        }

        public async Task<IEnumerable<NotificationModel<T>>> ListActive<T>(int userId)
        {
            using var unitOfWork = await UnitOfWorkFactory.CreateReadonly();
            var items = await unitOfWork.Notifications.ListActive<T>(userId);
            return items.Select(item => new NotificationModel<T>
            {
                Id = item.Id,
                UserId = item.UserId,
                Body = item.Body,
                Type = (NotificationType)item.Type,
                Status = (NotificationStatus)item.Status,
                CreateTime = item.CreateTime,
                UpdateTime = item.UpdateTime
            });
        }
    }
}