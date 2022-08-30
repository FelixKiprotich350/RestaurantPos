using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.BusinessModels.Security; 
using System.Windows; 

namespace RestaurantManager
{
    public static class ErpShared
    { 
        public static DateTime CurrentDate()
        {
            return DateTime.Now;
        }
        public static MainWindow Main_Window = null;
        public static PosUser CurrentUser = null;
        public static string DbConnectionstring = "server=127.0.0.1;user=root;password=toor;port=3306;database=restpos"; 
        public enum StudentStatuses
        {
            Admitted,
            Active,
            Deferred,
            Transferred,
            Deleted,
            Completed,
            Graduated
        }
        public enum FeeStructureStages
        {
            Preparation,
            Submitted,
            Approved,
            Deleted
        }
        public enum AccountsTransactionType
        {
            Debit, 
            Credit
        }

        public static Exception LastException = null;
        public static void ShowException(Exception ex)
        {
            if (ErpShared.LastException != null)
            {
                MessageBox.Show(ex.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void ShowException()
        {
            if (ErpShared.LastException != null)
            {
                MessageBox.Show(LastException.Message, "Message Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
