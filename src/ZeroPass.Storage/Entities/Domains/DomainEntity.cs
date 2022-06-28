using System;

namespace ZeroPass.Storage.Entities
{
    public enum DomainType : int
    {
        Personal = 1,
        Enterprise = 2
    }

    public class DomainEntity : EntityBase
    {
        public DomainType DomainType { get; set; }

        public string DomainName { get; set; }

        public string Company { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
