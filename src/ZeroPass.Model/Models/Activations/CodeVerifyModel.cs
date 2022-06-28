using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class CodeVerifyModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
