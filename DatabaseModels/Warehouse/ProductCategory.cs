using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Warehouse
{
   public class ProductCategory
    {
        //constructors
       
        //properties
        [Key]
        [MaxLength(100)]
        public string CategoryGuid { get; set; }
        [Required]
        [MaxLength(200)]
        [Index(IsUnique =true)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Department { get; set; }
        [Required] 
        public DateTime CreationDate { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public List<MenuProductItem> MenuItems { get; set; }
         
      
    }
}
