using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    [Serializable]
    public class DomainRegisterModel
    {
        [Required]
        public string DomainName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
