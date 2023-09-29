using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class LpoMaster
    {
        [Key]
        [MaxLength(100)]
        public string LpoGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string LPOID { get; set; } 
        [Required] 
        public string LPODescription { get; set; } 
        [Required] 
        [MaxLength(100)]
        public string UOM { get; set; } 
        [Required] 
        [MaxLength(100)]
        public string typeofasset { get; set; } 
        [Required]
        public decimal AssetItemCost { get; set; }  
        [Required]
        public int InStockQuantity { get; set; }
        [Required]
        public bool IsCancelled { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime ExpectedDate { get; set; }
         
        /// <summary>
        /// NOT MAPPED
        /// </summary>
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
