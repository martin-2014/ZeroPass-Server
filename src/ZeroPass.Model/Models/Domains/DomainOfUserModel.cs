namespace ZeroPass.Model.Models
{
    public class DomainOfUserModel
    {
        public int DomainId { get; set; }

        public int DomainType { get; set; }

        public string DomainName { get; set; }

        public string Company { get; set; }

        public string Timezone { get; set; }

        public string Logo { get; set; }

        public bool IsOwner { get; set; }

        public bool IsAdmin { get; set; }
    }
}