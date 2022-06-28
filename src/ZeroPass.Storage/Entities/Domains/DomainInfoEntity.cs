using System;

namespace ZeroPass.Storage.Entities
{
    public class DomainInfoEntity
    {
        public int DomainId { get; set; }

        public string ContactPhone { get; set; }

        public string ContactPerson { get; set; }

        public string NumberOfEmployees { get; set; }

        public string Country { get; set; }

        public string Timezone { get; set; }

        public string Logo { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
