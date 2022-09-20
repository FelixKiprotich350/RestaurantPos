﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Warehouse
{
    public class MenuProductItem
    {
        [Key]
        [MaxLength(100)]
        public string ProductGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(100)]
        public string CategoryGuid { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string AvailabilityStatus { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int RemainingQuantity { get; set; }
        //properties not mapped 
        [MaxLength(100)]
        [NotMapped]
        public string CategoryName { get; set; }
    }
}