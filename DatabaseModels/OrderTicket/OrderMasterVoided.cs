using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.OrderTicket
{

   public class OrderMasterVoided
    {
        public OrderMasterVoided()
        {
            IsSelected = false;
        }
        [Key]
        [MaxLength(100)]
        public string OrderGuid { get; set; }
        [Index]
        [Required]
        [MaxLength(100)]
        public string OrderNo { get; set; } 
      
        [MaxLength(200)]
        public string CustomerRefference { get; set; }
       
        [MaxLength(100)]
        public string TicketTable { get; set; }
        [Required]
        [MaxLength(100)]
        public string SystemUser { get; set; } 
        [Required]
        [MaxLength(100)]
        public string Workperiod { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } 
        [NotMapped]
        public bool IsSelected { get; set; }
        /// <summary>
        /// just placeholder but not a must to be used
        /// </summary>
        [NotMapped]
        public decimal Total { get; set; }
    }
}
