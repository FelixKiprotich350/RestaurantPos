using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payments
{
    public class TicketPaymentItem
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ParentSourceRef { get; set; }
        [Required]
        [MaxLength(100)]
        public string MasterTransNo { get; set; }
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public decimal AmountUsed { get; set; }
        [Required]
        public decimal PaymentBalance { get; set; }
        [Required]
        [MaxLength(100)]
        public string Method { get; set; }
        [Required]
        [MaxLength(100)]
        public string PayForSource { get; set; }
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

        [Required]
        [MaxLength(100)]
        public string Workperiod { get; set; } 


        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public bool IsInvoiceApproved { get; set; }
    }
}
