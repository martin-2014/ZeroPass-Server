using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class UserProfileUpdateModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Timezone { get; set; }

        public string Photo { get; set; }
    }
}