using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class UserPublicKeyModel
    {
        [Required]
        public string ExchangeKeyId { get; set; }

        [Required]
        public string PublicKey { get; set; }

        [Required]
        public string AdditionalData { get; set; }
    }
}