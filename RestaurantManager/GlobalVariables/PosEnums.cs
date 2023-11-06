using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.GlobalVariables
{
    public static class PosEnums
    {

        /// <summary>
        /// point of sale
        /// </summary>
        public enum OrderTicketStatuses
        {
            Pending,
            Voided,
            Closed,
            Completed
        }

        public enum TicketPaymentMethods
        {
            Cash,
            Mpesa,
            Card,
            Voucher,
            Invoice
        }
        public enum PaymentForSources
        {
            InvoicePay,
            SalesBill
        }

        public enum VoucherTypes
        {
            CustomerRedeemed,
            ProductDiscount,
            SpecialDayOffer,
            BulkSales
        }

        public enum VoucherStatuses
        {
            Available,
            Redeemed,
            Expired
        }

        public enum InvoiceStatuses
        {
            Issued,  
            Rejected,
            Approved,
            Paid

        } 
        public enum InvoiceSources
        {
            SalesBill,
            Purchase
        }

        /// <summary>
        /// security module
        /// </summary>
         public enum UserAccountsRoles
        {
            Admin,
            Supervisor,
            Accountant,
            Waiter,
            Cashier,
            KitchenCook,
            StoreKeeper
        }
        /// <summary>
        /// customer manager module
        /// </summary>
        public enum CustomerAccountActionType
        {
            PointsAward,
            Redeem
        }
        public enum PersonType
        {
            Employee,
            Customer,
            Supplier
        }
        public enum PersonAccountStatus
        {
            Active,
            Disabled
        }



        /// <summary>
        /// Accounts module
        /// 
        /// <p>Open: If the work period is open</p>
        /// <p>Closed: If the work period is closed</p>
        /// </summary>
        public enum WorkPeriodStatuses
        {
            Open,
            Closed
        }


        /// <summary>
        /// general items
        /// </summary>
        public enum SwitchMainWindow
        {
            SalesPoint,
            BackendSide
        }
        public enum Departments
        {
            Restaurant,
            Bar,
            Services
        }
        /// <summary>
        /// INVENTORY
        /// </summary>
        public enum StockFlowTriggerSource
        {
            Purchased,
            Sold, 
            Adjusted
        }
        public enum StockFlowAdjustmentReason
        {
            Expired,
            Faulty, 
            Missing,
            Others
        }
        /// <summary>
        /// Types of assets available
        /// </summary>
        public enum AssetTypes
        {
            FixedAsset,
            Consumable
        }
    }
}
