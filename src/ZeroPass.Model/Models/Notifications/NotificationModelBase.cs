using System;
using System.Text.Json.Serialization;

namespace ZeroPass.Model.Models
{
    public class NotificationModelBase<T>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public T Body { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationType Type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public enum NotificationType
    {
        InvitationReceived = 1,
        InvitationAccepted,
        InvitationRejected,
        UserJoinApproved,
        UserJoinRejected
    }

    public enum NotificationStatus
    {
        Created = 1,
        Processed  =2
    }
}