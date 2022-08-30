using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.GeneralSettings
{
    class TableEntity
    {

        [Key]
        [MaxLength(100)]
        [Column(Order = 0)]
        public string TableGuid { get; set; }
        
        [Required]
        [MaxLength(100)]
        [Column(Order = 1)]
        public string TableName { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 2)]
        public string TableStatus { get; set; }
        [Required]
        [Index]
        [MaxLength(100)]
        [Column(Order = 3)]
        public string IsDeleted { get; set; } 
        [Required]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }
    }
}
