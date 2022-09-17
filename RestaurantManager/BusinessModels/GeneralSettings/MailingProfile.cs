using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.GeneralSettings
{
    public class MailingProfile
    {
        [Key]
        [MaxLength(100)]
        public string RowGuid { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProfileName { get; set; }
        [Required]
        [MaxLength(200)]
        public string DisplayName { get; set; } 
        [Required]
        [MaxLength(200)]
        public string DestinationAddress { get; set; } 
        [Required]
        [MaxLength(200)]
        public string SenderAddress { get; set; } 
        [Required]
        [MaxLength(200)]
        public string MailingAddress { get; set; } 
        [Required]
        [MaxLength(200)]
        public string AppPassword { get; set; } 
    }
}
