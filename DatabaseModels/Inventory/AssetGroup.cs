using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
   public class AssetGroup
    {
        //constructors
       
        //properties
        [Key]
        [MaxLength(100)]
        public string GroupGuid { get; set; }
        [Required]
        [MaxLength(200)]
        [Index(IsUnique =true)]
        public string GroupName { get; set; } 
        [Required] 
        public DateTime CreationDate { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; } 
         
      
    }
}
