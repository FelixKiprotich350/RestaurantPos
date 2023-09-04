using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.OrderTicket
{
    public class OrderItem
    {
        [Key]
        [MaxLength(100)]
        public string ItemRowGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductItemGuid { get; set; }
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
        public bool IsGiftItem { get; set; }
        /// <summary>
        /// If the item is a gift - default value is None
        /// If the item is not a gift and it has a gift item  use the giftitem_productid
        /// </summary>
        [Required]
        public string GiftItemGuid { get; set; }
        /// <summary>
        /// If the item is a gift - use the productid of the Product which birthed this gift item
        /// If the item is not a gift default value is None
        /// </summary>
        [Required]
        public string ParentItemGuid { get; set; }
        [Required]
        public decimal DiscPercent { get; set; }
        [Required]
        public bool IsItemVoided { get; set; }

    }
}