using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    [Serializable]
    public class UserRegisterModel
    {
        [Required]
        public UserType AccountType { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Timezone { get; set; }
    }
}
