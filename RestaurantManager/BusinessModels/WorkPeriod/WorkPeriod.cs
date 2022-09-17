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
        public string WorkPeriodGuid { get; set; }
        [Required]
        [Index]
        [MaxLength(100)]
        public string WorkperiodName { get; set; }
        [Required]
        [MaxLength(100)]
        public string WorkperiodStatus { get; set; }
        [Required]
        [MaxLength(500)]
        public string OpeningNote { get; set; }
        [Required]
        [MaxLength(100)]
        public string Openedby { get; set; }
        [Required]
        public DateTime OpeningDate { get; set; }
        //[Required]
        [MaxLength(100)]
        public string ClosedBy { get; set; }
        [Required]
        public DateTime ClosingDate { get; set; }
        [Required]
        [MaxLength(500)]
        public string ClosingNote { get; set; }

        //not mapped properties
        [NotMapped]
        public int TotalTicketsCount { get; set; }
        [NotMapped]
        public int CompletedTicketsCount { get; set; }
        [NotMapped]
        public int VoidTicketsCount { get; set; }
        [NotMapped]
        public int PendingTicketsCount { get; set; }
    }
}
