using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class MenuProductItem
    {
        [Key]
        [MaxLength(100)]
        public string ProductGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryGuid { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string AvailabilityStatus { get; set; } 
        [Required] 
        [MaxLength(100)]
        public string Department { get; set; } 
        [Required]
        public decimal BuyingPrice { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        [Required]
        public decimal PackagingCost { get; set; }
        [Required]
        public decimal TotalCost { get; set; }
        [Required]
        public int RemainingQuantity { get; set; }
        [Required]
        public bool IsPrecount { get; set; }
        
        //properties not mapped 
        [MaxLength(100)]
        [NotMapped]
        public string CategoryName { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public int SoldQuantity { get; set; }
        [NotMapped]
        public int ReceivedQuantity { get; set; } 
        [NotMapped]
        public string AvailableQuantity { get; set; } 
        [NotMapped]
        public int PendingQuantity { get; set; } 
    }
}
