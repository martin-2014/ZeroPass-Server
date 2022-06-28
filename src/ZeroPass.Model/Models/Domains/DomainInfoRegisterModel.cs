using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    [Serializable]
    public class DomainInfoRegisterModel : DomainRegisterModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string ContactPhone { get; set; }

        [Required]
        public string ContactPerson { get; set; }

        [Required]
        public string NumberOfEmployees { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Timezone { get; set; }
    }
}
