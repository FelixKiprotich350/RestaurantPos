using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Vouchers
{
   public class DiscountedItem
    {
        [Key]
        [MaxLength(100)]
        public string ItemRowGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string BatchNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductItemGuid { get; set; }  

        //properties not mapped  
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
