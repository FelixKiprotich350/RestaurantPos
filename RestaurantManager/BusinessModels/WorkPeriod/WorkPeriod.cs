using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.WorkPeriod
{
    public class WorkPeriod
    {
        [Key]
        [MaxLength(100)]
        public string   WorkPeriodGuid { get; set; }
        [Required]
        [Index]
        [MaxLength(100)]
        public string WorkperiodName { get; set; }
        [Required]
        [MaxLength(500)]
        public string WorkperiodDescription { get; set; }
        [Required]
        [MaxLength(100)]
        public string WorkperiodStatus { get; set; }
        [Required]
        [MaxLength(100)]
        public string Openedby { get; set; }
        //[Required]
        [MaxLength(100)]
        public string ClosedBy { get; set; }
        [Required]
        public DateTime OpeningDate { get; set; }
        [Required]
        public DateTime ClosingDate { get; set; }
    }
}
