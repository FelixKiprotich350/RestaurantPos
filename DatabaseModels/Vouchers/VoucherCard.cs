using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Vouchers
{
    public class VoucherCard
    {
        [Key]
        [MaxLength(100)]
        public string VoucherGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string VoucherNumber { get; set; }
        [Required]
        [MaxLength(100)]
        public string VoucherBatchNo { get; set; }
        [Required] 
        [MaxLength(100)]
        public string VoucherStatus { get; set; }
        [Required] 
        [MaxLength(100)]
        public string VoucherType { get; set; }
        [Required]
        public decimal VoucherAmount { get; set; }
        [Required] 
        public DateTime CreationDate { get; set; }
        [Required] 
        public DateTime ExpiryDate { get; set; }
        public DateTime RedemptionDate { get; set; }

        //properties not mapped  
        [NotMapped] 
        public bool IsSelected { get; set; }
    }
}
