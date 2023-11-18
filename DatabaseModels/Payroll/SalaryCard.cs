using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payroll
{
    public class SalaryCard
    {
        [Key]
        [MaxLength(100)]
        public string CardGuid { get; set; } 
        [Required]
        [MaxLength(100)]
        [Index(IsUnique =true)]
        public string EmployeeNo { get; set; }
        [Required]
        public bool UseGroupRates { get; set; }
        [Required]
        [MaxLength(20)]
        public string PayGroupID { get; set; }
        [Required]
        public decimal MonthlyPay { get; set; } 
        [Required]
        public decimal DailyPay { get; set; }    
        [Required]
        [MaxLength(20)]
        public string HousingType { get; set; } 
        [Required] 
        public decimal HousingAllowance { get; set; }
        [Required]
        [MaxLength(20)]
        public string TaxPin { get; set; }
        public bool IsResident { get; set; }
        [Required]
        public bool MustDeductPaye { get; set; }
        [Required]
        public bool AllowDisabilityExcemption { get; set; }
        [Required]
        public decimal InsuranceReliefAmount { get; set; }
        [Required]
        [MaxLength(20)]
        public string NSSFNO { get; set; } 
        [Required]
        public bool MustDeductNSSF { get; set; }
        [Required]
        [MaxLength(20)]
        public string NHIFNO { get; set; }
        [Required]
        public bool MustDeductNHIF { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string BankAccNo { get; set; }
        [Required]
        [MaxLength(20)]
        public string BankID{ get; set; } 
        [Required]
        [MaxLength(20)]
        public string MpesaNumber{ get; set; } 
    }
}
