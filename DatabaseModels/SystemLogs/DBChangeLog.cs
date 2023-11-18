using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.SystemLogs
{
    public class DBChangeLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        [Required]
        public string LogActionType { get; set; }
        [Required]
        public string EntityName { get; set; }
        [Required]
        public string PropertyName { get; set; }
        [Required]
        public string PrimaryKeyValue { get; set; }
        [Required]
        public string OldValue { get; set; }
        [Required]
        public string NewValue { get; set; }
        [Required]
        public string SystemUser { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
