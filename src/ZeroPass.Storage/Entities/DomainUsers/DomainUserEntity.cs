using System;

namespace ZeroPass.Storage.Entities
{
    public enum UserStatus : int
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
    }

    [Serializable]
    public class DomainUserEntity
    {
        public int DomainId { get; set; }

        public int UserId { get; set; }

        public bool IsOwner { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive
        {
            get { return Status == UserStatus.Active; }
            set { Status = value ? UserStatus.Active : UserStatus.Inactive; }
        }

        public UserStatus Status { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
