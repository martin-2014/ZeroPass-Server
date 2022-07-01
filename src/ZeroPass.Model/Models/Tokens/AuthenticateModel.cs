using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class AuthenticateModel
    {
        [Required]
        public UserKeyRequestModel Request { get; set; }

        [Required]
        public string KeyId { get; set; }
    }
}