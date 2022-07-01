using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class UserKeyRequestModel
    {
        [Required]
        public ClientIdentifierProofModel ClientIdentifierProof { get; set; }

        [Required]
        public string Raw { get; set; }

        [Required]
        public string Signature { get; set; }
    }
}