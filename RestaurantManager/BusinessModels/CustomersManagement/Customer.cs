using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.CustomersManagement
{
    public class Customer
    {
        [Key]
        [MaxLength(100)]
        public string CustomerGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; } 
        [Required]
        [MaxLength(100)]
        public string PhoneNumber { get; set; } 
        [MaxLength(200)]
        public string CustomerEmail { get; set; }
        [Required]
        [MaxLength(100)]
        public string Gender { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }

        public DateTime BirthDate { get; set; }

        //properties not mapped  
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
