using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.GeneralSettings
{
    public class TableEntity
    {

        [Key]
        [MaxLength(100)] 
        public string TableGuid { get; set; }
        
        [Required]
        [MaxLength(100)] 
        [Index(IsUnique = true)]
        public string TableName { get; set; }
        [Required]
        [MaxLength(100)] 
        public string TableStatus { get; set; }
        [Required]
        [Index]  
        public bool IsDeleted { get; set; } 
        [Required] 
        public DateTime RegistrationDate { get; set; }
    }
}
