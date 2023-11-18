using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Payroll
{
    public class EmployeeAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpRecordID { get; set; }
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string EmployeeNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(200)]
        public string OtherNames { get; set; }
        [Required]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string NationalID { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }
       // [Required] 
        public DateTime RegistrationDate { get; set; }
        //[Required]
        public DateTime LastUpdatedon { get; set; }
        //[Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [MaxLength(20)]
        public string AccountStatus { get; set; }
        [Required]
        public string PhysicalAddress { get; set; }
        [Required]
        [MaxLength(20)]
        public string JobTitleID { get; set; }
        [Required]
        [MaxLength(20)]
        public string JobDepartmentID { get; set; }

        //[Required]
        public DateTime JobStartedDate { get; set; }


        //properties not mapped  

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
