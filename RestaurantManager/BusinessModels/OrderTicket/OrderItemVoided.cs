﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.OrderTicket
{
    public class OrderItemVoided
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
        [MaxLength(500)]
        public string VoidReason { get; set; }
        [Required]
        [MaxLength(100)]
        public string ApprovedBy { get; set; }
        [Required]
        [MaxLength(200)]
        public string WorkPeriod { get; set; }
        [Required] 
        public DateTime VoidTime { get; set; }

    }
}
