using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class UserKeyCreateModel
    {
        public int UserId { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Verifier { get; set; }

        [Required]
        public string PublicDataKey { get; set; }

        [Required]
        public string PrivateDataKey { get; set; }
    }
}
