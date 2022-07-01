using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class UserExchangePublicKeyModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PublicKey { get; set; }
    }
}