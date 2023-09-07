using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.GlobalVariables
{
    public static class PosEnums
    {
        public enum OrderTicketStatuses
        {
            Pending,
            Voided,
            Merged,
            Completed
        }
        public enum TicketPaymentMethods
        {
            Cash,
            Mpesa,
            Card,
            Voucher
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

        public enum UserAccountStatuses
        {
            Active,
            Disabled
        }
        public enum CustomerAccountActionType
        {
            PointsAward,
            Redeem
        }
        /// <summary>
        /// Work Period statuses
        /// 
        /// <p>Open: If the work period is open</p>
        /// <p>Closed: If the work period is closed</p>
        /// </summary>
        public enum WorkPeriodStatuses
        {
            Open,
            Closed
        }
        public enum SwitchMainWindow
        {
            SalesPoint,
            BackendSide
        }public enum UserAccountsRoles
        {
            Admin,
            Supervisor,
            Accountant,
            Waiter,
            Cashier,
            KitchenCook,
            StoreKeeper
        }
    }
}
