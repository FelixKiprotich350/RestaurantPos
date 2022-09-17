using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.WorkPeriod
{
    public class WPClosureMessage
    {
        public string WorkPeriodName { get; set; } 
        public string OpenedBy { get; set; }
        public string OpeningTime { get; set; }
        public string ClosedBy { get; set; }
        public string ClosingTime { get; set; }
        public string PendingTickets { get; set; }
        public string VoidedTickets { get; set; }
        public string CompletedTickets { get; set; }
        public string Cash { get; set; }
        public string Mpesa { get; set; }
        public string Vouchers { get; set; }
        public string Cards { get; set; }
        public string Total { get; set; }
        public string OpeningNote { get; set; }
        public string ClosingNote { get; set; }
    }
}
