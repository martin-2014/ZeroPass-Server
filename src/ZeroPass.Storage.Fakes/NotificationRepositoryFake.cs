using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Storage.Fakes
{
public class NotificationRepositoryFake : INotificationRepository
    {
        readonly FakeDatabase Database;
        List<NotificationEntity<JsonElement, JsonElement>> Notifications
            => Database.Notifications;

        public NotificationRepositoryFake(FakeDatabase database)
        { 
            Database = database;
        }

        public async Task<NotificationEntity<T>> Get<T>(int id)
        {
            var n = Notifications.FirstOrDefault(n => n.Id == id);
            if (n == null) return null;

            return await Task.FromResult(ConvertToGeneric<T>(n));
        }

        public Task Insert<T>(NotificationEntity<T> value)
        {
            var entity = ConvertFromGeneric(value);
            entity.Id = Database.AllocateNotificationId();
            Notifications.Add(entity);
            return Task.CompletedTask;
        }

        public async Task Insert<T>(IEnumerable<NotificationEntity<T>> values)
        {
            foreach (var v in values)
            {
                await Insert(v);
            }
        }

        public Task<IEnumerable<NotificationEntity<T>>> ListActive<T>(int userId)
        {
            var ns = Notifications.Where(n => n.UserId == userId && n.Status == 1);
            return Task.FromResult(ns.Select(ConvertToGeneric<T>));
        }

        public Task Process(int userId, IEnumerable<int> ids, int newStatus, JsonElement result)
        {
            var n = Notifications.First(n => n.UserId == userId && ids.Contains(n.Id));
            n.Status = newStatus;
            n.Result = result;
            return Task.CompletedTask;
        }

        public Task SetStatus(int userId, IEnumerable<int> ids, int newStatus)
        {
            Notifications.Where(n => n.UserId == userId && ids.Contains(n.Id)).ToList().ForEach(n => n.Status = newStatus);
            return Task.CompletedTask;
        }
        static NotificationEntity<T> ConvertToGeneric<T>(NotificationEntity<JsonElement, JsonElement> value)
        {
            var result = new NotificationEntity<T>
        {
                Id = value.Id,
                UserId = value.UserId,
                Type = value.Type,
                Status = value.Status,
                CreateTime = value.CreateTime,
                Body = value.Body.To<T>()
            };
            return result;
        }

        static NotificationEntity<JsonElement, JsonElement> ConvertFromGeneric<TBody>(NotificationEntity<TBody> value)
        {
            var entity = new NotificationEntity<JsonElement, JsonElement>
        {
                Id = value.Id,
                UserId = value.UserId,
                Type = value.Type,
                Status = value.Status,
                CreateTime = value.CreateTime,
                Body = JsonDocument.Parse(JsonSerializer.Serialize(value)).RootElement
            };
            return entity;
        }
    }
}