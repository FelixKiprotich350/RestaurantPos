using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.OrderTicket
{

   public class OrderMaster
    {
        public OrderMaster()
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
        [Required]
        [MaxLength(100)]
        public string OrderStatus { get; set; }
      
        [MaxLength(200)]
        public string CustomerRefference { get; set; }
       
        [MaxLength(100)]
        public string TicketTable { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserServing { get; set; }
        [Required]
        [MaxLength(500)]
        public string VoidReason { get; set; } 
        //[Required]
        //public bool IsKitchenServed { get; set; }

        //[Required]
        //public bool IsInPreparation { get; set; }
        /// <summary>
        /// This field is used when the ticket was derived from merged tickets
        /// <p>Its an ID of the original Ticket</p>
        /// </summary> 
        [MaxLength(100)]
        public string MergedChild { get; set; }
        [Required]
        [MaxLength(100)]
        public string Workperiod { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public bool IsPrinted { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
