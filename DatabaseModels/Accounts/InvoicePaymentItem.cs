using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payments
{
    public class InvoicePaymentItem
    {
        [Key]
        [MaxLength(100)]
        public string PaymentGuid { get; set; } 
        [Required]
        [MaxLength(100)]
        public string InvoiceNo { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        [MaxLength(100)]
        public string Method { get; set; }
        [Required]
        [MaxLength(100)]
        public string PaymentWorkperiod { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }  

        [Required]
        [MaxLength(100)]
        public string ReceivingUsername { get; set; } 


        [NotMapped]
        public bool IsSelected { get; set; } 
    }
}
