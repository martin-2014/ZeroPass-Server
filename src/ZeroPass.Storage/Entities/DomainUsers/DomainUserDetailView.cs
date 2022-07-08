using ZeroPass.Model.Models;

namespace ZeroPass.Storage.Entities
{
    public partial class DomainUserDetailView
    {
        public int DomainId { get; set; }

        public DomainType DomainType { get; set; }

        public string DomainName { get; set; }

        public string Company { get; set; }

        public string Timezone { get; set; }

        public string Logo { get; set; }

        public bool IsOwner { get; set; }

        public bool IsAdmin { get; set; }

        public UserStatus Status { get; set; }
    }
}