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
        public string ClientGuid { get; set; }
        [Required]
        [MaxLength(100)] 
        public string ClientTitle { get; set; }
        [Required]
        [MaxLength(100)] 
        public string PhysicalAddress { get; set; }
        [Required]
        [Index]
        [MaxLength(100)] 
        public string Email { get; set; }
        [Required]
        [MaxLength(100)] 
        public string Phone { get; set; }
        [Required]
        [MaxLength(100)] 
        public string ClientKRAPIN { get; set; }
        [Required]
        [MaxLength(100)] 
        public string ReceiptNote1 { get; set; }
        [Required]
        [MaxLength(100)] 
        public string ReceiptNote2 { get; set; }
        [Required]
        [MaxLength(100)] 
        public string ReceiptNote3 { get; set; }
        [Required]  
        public decimal TaxPercentage { get; set; }
        [Required]  
        public string AcceptedCards { get; set; }
    }
}
