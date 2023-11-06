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
        public string AssetDescription { get; set; }
        [Required]
        [MaxLength(100)]
        public string AssetGroupGuid { get; set; }
        [Required] 
        [MaxLength(100)]
        public string UOM { get; set; } 
        [Required]   
        public bool IsFoodMaterial { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }

        //properties not mapped  
        [NotMapped]
        public string GroupName { get; set; }
        [NotMapped]
        public string UOMName { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
