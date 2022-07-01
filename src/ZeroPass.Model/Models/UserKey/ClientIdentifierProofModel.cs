using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class ClientIdentifierProofModel
    {
        [Required]
        public string Email { get; set; }

        //SRP Identifier of clinet
        [Required]
        public string IdentifierProof { get; set; }
    }
}