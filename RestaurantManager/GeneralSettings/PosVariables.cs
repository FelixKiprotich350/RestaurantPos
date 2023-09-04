using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.GeneralSettings
{
    public class PosVariables
    {
        [Key]
        [MaxLength(100)] 
        public string RowGuid { get; set; }

        [Required]
        [MaxLength(500)] 
        public string VarName { get; set; }
        [Required]
        [MaxLength(500)] 
        public string VarValue1 { get; set; }
        [Required]
        [MaxLength(500)] 
        public string VarValue2 { get; set; }
    }
}
