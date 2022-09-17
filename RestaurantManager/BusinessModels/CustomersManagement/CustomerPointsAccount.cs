using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.CustomersManagement
{
    public class CustomerAccount
    {
        [Key]
        [MaxLength(100)]
        public string AccActionGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string TransactionNo { get; set; }
        [Required]
        [MaxLength(200)]
        public string CustomerPhoneNo { get; set; }
        [Required]
        public int Debit { get; set; }
        [Required]
        public int Credit { get; set; } 
        [Required]
        public DateTime ActionDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string ApprovedBy { get; set; }
        [Required]
        [MaxLength(100)]
        public string TransactionType { get; set; }

        //properties not mapped     
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
