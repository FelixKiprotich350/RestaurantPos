using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class AssetItem
    {
        [Key]
        [MaxLength(100)]
        public string AssetItemGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string AssetName { get; set; }
        [Required]
        [MaxLength(100)]
        public string AssetDepartment { get; set; }
        [Required] 
        public string AssetDescription { get; set; }
        [Required]
        [MaxLength(100)]
        public string AssetGroupGuid { get; set; }  
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
        public bool IsPrecount { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }

        //properties not mapped 
        [MaxLength(100)]
        [NotMapped]
        public string GroupName { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
