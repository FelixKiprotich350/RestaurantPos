using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.SystemLogs
{
    public class UserActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        [Required]
        [MaxLength(100)]
        public string Logtype { get; set; }
        [Required]
        [MaxLength(100)]
        public string LogLevel { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } 
        public string Parameters { get; set; }
        [Required]
        public string SystemUser { get; set; } 
        [Required]
        public DateTime Timmestamp { get; set; }
         
        

    }
}