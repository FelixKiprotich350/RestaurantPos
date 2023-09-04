using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payments
{

    /// <summary>
    /// Not mapped to databse 
    /// Only to keep records of payments when checking out
    /// </summary>
    public class PaymentMethod
    {
        [Key]
        [MaxLength(100)]
        public string PaymentMethodName { get; set; }
        [Required] 
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(100)]
        public string PrimaryRefference { get; set; }  
        [Required]
        [MaxLength(500)]
        public string SecondaryRefference { get; set; }  
    }
}
