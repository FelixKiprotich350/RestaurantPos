using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.OrderTicket
{
    public class Un_OrderItem
    {
        [Key]
        [MaxLength(100)]
        public string ParentProductItemGuid { get; set; }  
        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; }
        [Required]
        public int Quantity { get; set; } 
        [Required]
        public decimal Total { get; set; } 
        public decimal BuyingPriceTotal { get; set; } 
         
    }
}