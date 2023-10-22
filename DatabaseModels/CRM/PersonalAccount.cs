using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.CRM
{
    public class PersonalAccount
    {
        [Key]
        [MaxLength(100)]
        public string PersonGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string AccountNo { get; set; }
        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string NationalID { get; set; }
        [Required]
        public decimal InvoiceLimit { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; } 
        [Required]
        [MaxLength(100)]
        public string Gender { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string AccountStatus { get; set; }

        //properties not mapped  

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
