using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Payments
{
    public class TicketPaymentMaster
    {
        [Key]
        [MaxLength(100)]
        public string PaymentMasterGuid { get; set; }
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
