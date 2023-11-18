using DatabaseModels.Security;
using RestaurantManager.UserInterface.Accounts;
using RestaurantManager.UserInterface.CustomersManagemnt;
using RestaurantManager.UserInterface.GeneralSettings;
using RestaurantManager.UserInterface.PointofSale;
using RestaurantManager.UserInterface.PosReports;
using RestaurantManager.UserInterface.Security;
using RestaurantManager.UserInterface.Inventory;
using RestaurantManager.UserInterface.WareHouseReports; 
using RestaurantManager.UserInterface.WorkPeriods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.UserInterface.PosReports.Payments;
using RestaurantManager.UserInterface.PosReports.WareHouseReports;
using RestaurantManager.UserInterface.PosReports.SalesReport;
using RestaurantManager.UserInterface.HR;
using RestaurantManager.UserInterface.Inventory.AssetManagement;
using RestaurantManager.UserInterface.Security.AuditReports;

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
                     new PermissionMaster() { PermissionCode = "A6", ParentModule = "A", PermissionShortName = "Price List", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new PriceList() },
                     new PermissionMaster() { PermissionCode = "A7", ParentModule = "A", PermissionShortName = "Pay Invoice", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new CheckoutInvoice() },
                     new PermissionMaster() { PermissionCode = "A8", ParentModule = "A", PermissionShortName = "Void Ticket", PermissionFullName = "Description", PermissionLevel = "2", PageClass = null },
                     new PermissionMaster() { PermissionCode = "A9", ParentModule = "A", PermissionShortName = "Subtract Quantity", PermissionFullName = "Description", PermissionLevel = "2", PageClass = null },
                     //Work periods
                     new PermissionMaster() { PermissionCode = "B1", ParentModule = "B", PermissionShortName = "Periods List", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ViewWorkPeriod() },
                     new PermissionMaster() { PermissionCode = "B2", ParentModule = "B", PermissionShortName = "Manage Period", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageWorkPeriod() },
                     //Accounts
                      new PermissionMaster() {PermissionCode = "C1", ParentModule = "C", PermissionShortName = "Accounts Dashboard", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new AccountsDashboard() },
                     new PermissionMaster() { PermissionCode = "C2", ParentModule = "C", PermissionShortName = "Accounts Details", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new AccountTicketsPayments() },
                     new PermissionMaster() { PermissionCode = "C3", ParentModule = "C", PermissionShortName = "Vouchers List", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new VouchersList() },
                     new PermissionMaster() { PermissionCode = "C4", ParentModule = "C", PermissionShortName = "Invoice Accounts", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ViewInvoicableAccounts() },
                     new PermissionMaster() { PermissionCode = "C5", ParentModule = "C", PermissionShortName = "Invoices Master", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ViewInvoicesmaster() },
                     new PermissionMaster() { PermissionCode = "C6", ParentModule = "C", PermissionShortName = "Invoice Payments", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ViewInvoicesPayments() },
                     //products
                     new PermissionMaster() { PermissionCode = "D1", ParentModule = "D", PermissionShortName = "Menu Categories", PermissionFullName = "Menu Categories", PermissionLevel = "1", PageClass = new MenuCategories() },
                     new PermissionMaster() { PermissionCode = "D2", ParentModule = "D", PermissionShortName = "Menu Products", PermissionFullName = "Menu Products", PermissionLevel = "1", PageClass = new MenuProducts() },
                     new PermissionMaster() { PermissionCode = "D3", ParentModule = "D", PermissionShortName = "Asset Master", PermissionFullName = "Assets Master", PermissionLevel = "1", PageClass = new AssetsMaster() },
                      new PermissionMaster() { PermissionCode = "D4", ParentModule = "D", PermissionShortName = "Asset Purchase", PermissionFullName = "Requisition", PermissionLevel = "1", PageClass = new AssetPurchase() },
                     new PermissionMaster() { PermissionCode = "D5", ParentModule = "D", PermissionShortName = "Stock Control", PermissionFullName = "Local Purchase Order", PermissionLevel = "1", PageClass = new StockControlForm() },
                     new PermissionMaster() { PermissionCode = "D6", ParentModule = "D", PermissionShortName = "Coming Soon❤", PermissionFullName = "Receive LPO", PermissionLevel = "1", PageClass = "😂🤣😒" },
                     //Customers
                     new PermissionMaster() { PermissionCode = "E1", ParentModule = "E", PermissionShortName = "All Persons", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new PersonsList() },
                     new PermissionMaster() { PermissionCode = "E2", ParentModule = "E", PermissionShortName = "Customers", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new CustomerAccounts() },
                     new PermissionMaster() { PermissionCode = "E3", ParentModule = "E", PermissionShortName = "Suppliers", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                     
                     //human resource
                    new PermissionMaster() { PermissionCode = "F1", ParentModule = "F", PermissionShortName = "Employees", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                    new PermissionMaster() { PermissionCode = "F2", ParentModule = "F", PermissionShortName = "Payroll Master", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                    new PermissionMaster() { PermissionCode = "F3", ParentModule = "F", PermissionShortName = "Salary Groups", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                    new PermissionMaster() { PermissionCode = "F4", ParentModule = "F", PermissionShortName = "Statutories Master", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                    new PermissionMaster() { PermissionCode = "F5", ParentModule = "F", PermissionShortName = "Pay Schedule", PermissionFullName = "Description", PermissionLevel = "1",PageClass=new Employee() },
                     
                     
                     //reports
                     new PermissionMaster() { PermissionCode = "G1", ParentModule = "G", PermissionShortName = "Sales Report", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MasterReports() },
                     new PermissionMaster() { PermissionCode = "G2", ParentModule = "G", PermissionShortName = "Payments", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new Paymentsmaster() },
                     new PermissionMaster() { PermissionCode = "G3", ParentModule = "G", PermissionShortName = "Stock Report", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new WHMasterReports() },
                     new PermissionMaster() { PermissionCode = "G4", ParentModule = "G", PermissionShortName = "System Logs", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new LogsMaster() },
                     //settings
                     new PermissionMaster() { PermissionCode = "H1", ParentModule = "H", PermissionShortName = "Tables", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new TablesEntities() },
                     new PermissionMaster() { PermissionCode = "H2", ParentModule = "H", PermissionShortName = "Client Info", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ClientInfo() },
                     //new PermissionMaster() { PermissionCode = "H3", ParentModule = "H", PermissionShortName = "Mail Settings", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MailSettings() },
                     //new PermissionMaster() { PermissionCode = "H4", ParentModule = "H", PermissionShortName = "Database", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new DatabaseSettings() },
                     //new PermissionMaster() { PermissionGuid = "H5", ParentModule = "H", PermissionShortName = "User Roles", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                     new PermissionMaster() { PermissionCode = "H6", ParentModule = "H", PermissionShortName = "System Users", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageSystemUsers() },
                     new PermissionMaster() { PermissionCode = "H7", ParentModule = "H", PermissionShortName = "My Account", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MyProfile() },

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
