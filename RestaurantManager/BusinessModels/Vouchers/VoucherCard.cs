using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Vouchers
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
        public string VoucherBatch { get; set; }
        [Required] 
        [MaxLength(100)]
        public string VoucherStatus { get; set; }
        [Required]
        public decimal VoucherAmount { get; set; }
        [Required] 
        public DateTime VoucherCreationDate { get; set; }
        [Required] 
        public DateTime VoucherUsageDate { get; set; }

        //properties not mapped  
        [NotMapped] 
        public bool IsSelected { get; set; }
    }
}
