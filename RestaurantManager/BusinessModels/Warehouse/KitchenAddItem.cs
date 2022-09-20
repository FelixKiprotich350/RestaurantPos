using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Warehouse
{
   public class KitchenAddItem
    {
        //constructors
       
        //properties
        [Key]
        [MaxLength(100)]
        public string ItemGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string WorkPeriod { get; set; } 
        [Required]
        public int Quantity { get; set; }
        [Required] 
        public DateTime InsertionDate { get; set; }
        [Required] 
        [MaxLength(200)] 
        public string InsertionBy { get; set; }
        [NotMapped]
        public string ProductName { get; set; }

    }
}
