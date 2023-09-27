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

        /// <summary>
        /// security module
        /// </summary>
        public enum UserAccountStatuses
        {
            Active,
            Disabled
        }
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
        /// Types of assets available
        /// </summary>
        public enum AssetTypes
        {
            FixedAssets,
            Consumables
        }
    }
}
