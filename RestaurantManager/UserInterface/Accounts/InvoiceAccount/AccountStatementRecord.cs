using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 


namespace RestaurantManager.UserInterface.Accounts.InvoiceAccount
{
    public class AccountStatementRecord
    {
        public string InvNo { get; set; }
        public string PayRefNo { get; set; }
        public string ServedBy { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
    }
     

 

}
