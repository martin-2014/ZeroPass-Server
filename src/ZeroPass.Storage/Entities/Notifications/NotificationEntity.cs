using System;

namespace ZeroPass.Storage.Entities
{
    public class NotificationEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }

    public class NotificationEntity<T> : NotificationEntity
    {
        public T Body { get; set; }
    }

    public class NotificationEntity<TBody, TResult> : NotificationEntity<TBody>
    { 
        public TResult Result { get; set; }
    }
}