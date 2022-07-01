using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZeroPass.Model.Models
{
    public class NotificationClearModel
    {
        [Required]
        public IEnumerable<int> Ids { get; set; }
    }
}