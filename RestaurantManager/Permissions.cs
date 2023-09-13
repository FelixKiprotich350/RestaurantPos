using DatabaseModels.Security;
using RestaurantManager.UserInterface.Accounts;
using RestaurantManager.UserInterface.CustomersManagemnt;
using RestaurantManager.UserInterface.GeneralSettings;
using RestaurantManager.UserInterface.PointofSale;
using RestaurantManager.UserInterface.PosReports;
using RestaurantManager.UserInterface.Security;
using RestaurantManager.UserInterface.Warehouse;
using RestaurantManager.UserInterface.WorkPeriods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager
{
    public class Permissions
    {
        public List<PermissionMaster> GetAllPermissions()
        {
            try
            {
                List<PermissionMaster> p = new List<PermissionMaster>
                {

                     new PermissionMaster() { PermissionCode = "A1", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new Salespoint() },
                     new PermissionMaster() { PermissionCode = "A2", ParentModule = "A", PermissionShortName = "Update Ticket", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new EditTicket() },
                     new PermissionMaster() { PermissionCode = "A3", ParentModule = "A", PermissionShortName = "Manage Tickets", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageTickets() },
                     new PermissionMaster() { PermissionCode = "A4", ParentModule = "A", PermissionShortName = "Check Out", PermissionFullName = "Description", PermissionLevel = "1", PageClass =new CheckoutTicket() },
                     new PermissionMaster() { PermissionCode = "A5", ParentModule = "A", PermissionShortName = "View Tickets", PermissionFullName = "Description", PermissionLevel = "1", PageClass =new ViewTickets() },
                     new PermissionMaster() { PermissionCode = "A6", ParentModule = "A", PermissionShortName = "Void Ticket", PermissionFullName = "Description", PermissionLevel = "2", PageClass = null },
                     //Work periods
                     new PermissionMaster() { PermissionCode = "B1", ParentModule = "B", PermissionShortName = "Periods List", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ViewWorkPeriod() },
                     new PermissionMaster() { PermissionCode = "B2", ParentModule = "B", PermissionShortName = "Manage Period", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageWorkPeriod() },
                     //Accounts
                     new PermissionMaster() { PermissionCode = "C1", ParentModule = "C", PermissionShortName = "Accounts Dashboard", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new AccountsDashboard() },
                     new PermissionMaster() { PermissionCode = "C2", ParentModule = "C", PermissionShortName = "Accounts Details", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new AccountTicketsPayments() },
                     new PermissionMaster() { PermissionCode = "C3", ParentModule = "C", PermissionShortName = "Vouchers List", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new VouchersList() },
                     new PermissionMaster() { PermissionCode = "C4", ParentModule = "C", PermissionShortName = "Discounts", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new DiscountsManager() },
                     //products
                    new PermissionMaster() { PermissionCode = "D1", ParentModule = "D", PermissionShortName = "Menu Categories", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuCategories() },
                     new PermissionMaster() { PermissionCode = "D2", ParentModule = "D", PermissionShortName = "Menu Products", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuProducts() },
                     new PermissionMaster() { PermissionCode = "D3", ParentModule = "D", PermissionShortName = "Kitchen Adds", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new KitchenAdds() },
                     new PermissionMaster() { PermissionCode = "D4", ParentModule = "D", PermissionShortName = "Stock Entry", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new StockEntry() },
                     new PermissionMaster() { PermissionCode = "D5", ParentModule = "D", PermissionShortName = "Discounts", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new Discounts() },
                     //reports
                     new PermissionMaster() { PermissionCode = "E2", ParentModule = "E", PermissionShortName = "Reports", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MasterReports() },
                     //settings
                     new PermissionMaster() { PermissionCode = "F1", ParentModule = "F", PermissionShortName = "Tables", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new TablesEntities() },
                     new PermissionMaster() { PermissionCode = "F2", ParentModule = "F", PermissionShortName = "Client Info", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ClientInfo() },
                     //new PermissionMaster() { PermissionCode = "F3", ParentModule = "F", PermissionShortName = "Mail Settings", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MailSettings() },
                     //new PermissionMaster() { PermissionCode = "F4", ParentModule = "F", PermissionShortName = "Database", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new DatabaseSettings() },
                     //new PermissionMaster() { PermissionGuid = "F5", ParentModule = "F", PermissionShortName = "User Roles", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                     new PermissionMaster() { PermissionCode = "F6", ParentModule = "F", PermissionShortName = "System Users", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageSystemUsers() },
                     new PermissionMaster() { PermissionCode = "F7", ParentModule = "F", PermissionShortName = "My Account", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MyProfile() },
                     //Customers
                     new PermissionMaster() { PermissionCode = "H1", ParentModule = "H", PermissionShortName = "Customers", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new CustomersList() },
                     new PermissionMaster() { PermissionCode = "H2", ParentModule = "H", PermissionShortName = "Customer Account", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new CustomerAccount() }
                };
                return p;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
    }
}
