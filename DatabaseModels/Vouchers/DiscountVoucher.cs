using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Vouchers
{
    public class DiscountVoucher
    {
        [Key]
        [MaxLength(100)]
        public string BatchGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string BatchNumber { get; set; } 
        [Required]
        [MaxLength(100)]
        public string VoucherType { get; set; }
        [Required]
        public decimal VoucherAmount { get; set; }
        [Required]
        public decimal BulkSalesLimitAmount { get; set; }
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [MaxLength(500)]
        public string BatchDescription { get; set; }
        [Required]
        public bool IsActiveStatus { get; set; }

        //properties not mapped  
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
