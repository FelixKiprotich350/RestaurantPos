using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.GeneralSettings
{
    public class ClientInfoDetails
    {
        [Key]
        [MaxLength(100)]
        [Column(Order = 0)]
        public string ClientGuid { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 1)]
        public string ClientTitle { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 2)]
        public string PhysicalAddress { get; set; }
        [Required]
        [Index]
        [MaxLength(100)]
        [Column(Order = 3)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 4)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 5)]
        public string ClientKRAPIN { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 6)]
        public string ReceiptNote1 { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 7)]
        public string ReceiptNote2 { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 8)]
        public string ThankYouNote { get; set; }
    }
}
