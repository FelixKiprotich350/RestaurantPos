

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Accounts
{
    public class InvoicesMaster
    {
        [Key]
        [MaxLength(100)]
        public string InvoiceGuid { get; set; }
        [Required] 
        [MaxLength(100)]
        [Index(IsUnique =true)]
        public string InvoiceNo { get; set; } 
        [Required]
        public decimal InvoiceAmount { get; set; }
        [Required] 
        public string InvoiceStatus { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public DateTime LastPaymentDate { get; set; }
        [Required]
        public DateTime ExpectedPayDate { get; set; }
        [Required] 
        public string PrimaryRefference { get; set; }
        [Required] 
        public string InvoiceSource { get; set; }
        [Required] 
        public string CancelReason { get; set; }
        [Required] 
        public string Notes { get; set; }
        [Required]
        [MaxLength(100)]
        public string SystemUser { get; set; }
        [Required]
        [MaxLength(100)]
        public string CustomerAccNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string Workperiod { get; set; } 


        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
