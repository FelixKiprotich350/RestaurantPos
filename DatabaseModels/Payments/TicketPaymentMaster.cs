using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payments
{
    public class TicketPaymentMaster
    {
        [Key]
        [MaxLength(100)]
        public string PaymentMasterGuid { get; set; }
        [Required]
        [MaxLength(100)]
        [Index(IsUnique =true)]
        public string TransNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string TicketNo { get; set; }
        [Required] 
        public decimal TotalAmountPaid { get; set; }
        [Required] 
        public decimal TotalAmountCharged { get; set; }
        [Required]
        public decimal TicketBalanceReturned { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; } 
        [Required]
        [MaxLength(100)]
        public string PosUser { get; set; }
        [Required]
        [MaxLength(100)]
        public string WorkPeriod { get; set; }
    }
}
