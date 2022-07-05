using System.Text.Json;
using ZeroPass.Model.Models;
using ZeroPass.Storage.Entities;

namespace ZeroPass.Api.Tests
{
    public class TestNotification
    {
        public TestNotification(NotificationEntity<JsonElement, JsonElement> entity)
        {
            Entity = entity;
        }

        public readonly NotificationEntity<JsonElement, JsonElement> Entity;

        public NotificationStatus Status => (NotificationStatus)Entity.Status;

        public NotificationType Type => (NotificationType)Entity.Type;

        public int UserId => Entity.UserId;
    }
}