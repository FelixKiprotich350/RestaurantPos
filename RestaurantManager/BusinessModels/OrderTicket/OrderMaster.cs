using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.PointofSale
{

    class OrderMaster
    {

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
        public string CustomerName { get; set; }
       
        [MaxLength(100)]
        public string TicketTable { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserServing { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
