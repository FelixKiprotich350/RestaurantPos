using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.OrderTicket
{
    public class OrderItem
    {
        [Key]
        [MaxLength(100)]
        public string ItemRowGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ParentProductItemGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string OrderID { get; set; }
        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        [MaxLength(20)]
        public string ServiceType { get; set; }
        [Required]
        public bool IsDiscountItem { get; set; }
        [Required]
        public string ParentItem { get; set; }
        [Required]
        public decimal DiscPercent { get; set; }
        [Required]
        public bool IsItemVoided { get; set; }

    }
}