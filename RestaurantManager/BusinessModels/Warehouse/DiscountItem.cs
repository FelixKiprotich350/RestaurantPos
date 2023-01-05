using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Warehouse
{
    public class DiscountItem
    {
        [Key]
        [MaxLength(100)]
        public string DiscRowID { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductGuid { get; set; }
        [Required] 
        public string ProductName { get; set; }
        [MaxLength(100)]
        public string DiscType { get; set; }
        [Required]  
        public string AttachedProduct { get; set; } 
        [Required]
        public decimal DiscPercentage { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string OfferDay { get; set; }
        [Required]
        public bool IsRepetitive { get; set; }
        [Required]
        public string DiscStatus { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
