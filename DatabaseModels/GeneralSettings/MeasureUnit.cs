using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.GeneralSettings
{
    public class MeasureUnit
    {

        [Key]
        [MaxLength(100)] 
        public string UnitGuid { get; set; }
        
        [Required]
        [MaxLength(100)] 
        [Index(IsUnique = true)]
        public string UnitName { get; set; }
        [Required]
        [MaxLength(100)] 
        public string UnitShortcode { get; set; } 
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime LastUpdateDate { get; set; }
    }
}
