using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.HROffice
{
    public class EmployeeAccount
    {
        [Key]
        [MaxLength(100)]
        public string AccountGuid { get; set; } 
        [Required]
        [MaxLength(200)]
        [Index(IsUnique =true)]
        public string PersonAccNo { get; set; } 
        [Required]
        public decimal InvoiceLimit { get; set; }  
        [Required]
        public decimal MonthlySalary { get; set; }  
        [Required]
        public DateTime RegDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastUpdatedBy { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountStatus { get; set; }

        //properties not mapped     
        [NotMapped]
        public bool IsSelected { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        [NotMapped]
        public string PhoneNo { get; set; }
        [NotMapped]
        public string Gender { get; set; }
    }
}
