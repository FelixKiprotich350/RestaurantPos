using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Payments
{
    public class TicketPaymentItem
    {
        [Key]
        [MaxLength(100)]
        public string PaymentGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ParentOrderNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string ParentPaymasterGuid { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        [MaxLength(100)]
        public string Method { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string PrimaryRefference { get; set; }
        [Required]
        [MaxLength(500)]
        public string SecondaryRefference { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReceivingUsername { get; set; }

    }
}
