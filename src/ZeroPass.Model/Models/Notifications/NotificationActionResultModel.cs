using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ZeroPass.Model.Models
{
    public class NotificationActionResultModel
    {
        [Required]
        public IEnumerable<int> Ids { get; set; }

        [Required]
        public JsonElement Result { get; set; }
    }
}