using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Menu
{
    public class MenuProductItem
    { 
        [Key]
        [MaxLength(100)]
        public string ProductGuid { get; set; } 
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(20)]
        public string CategoryGuid { get; set; }
        [Required]  
        public decimal Price { get; set; }
    }
}
