using System.Collections.Generic;

namespace ZeroPass.Model.Models.UserProfiles
{
    public partial class UserProfileModel
    {
        public int Id { get; set; }

        public int UserType { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Timezone { get; set; }

        public IEnumerable<DomainOfUserModel> Domains { get; set; }
    }
}