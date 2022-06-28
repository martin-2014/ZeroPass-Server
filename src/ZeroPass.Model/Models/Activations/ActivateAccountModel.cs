using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class ActivateAccountModel : CodeVerifyResultModel
    {
        [Required]
        public UserKeyCreateModel UserKey { get; set; }
    }
}
