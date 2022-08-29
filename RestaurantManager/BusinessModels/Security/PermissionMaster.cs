using RestaurantManager.UserInterface.MenuProducts;
using RestaurantManager.UserInterface.PointofSale;
using RestaurantManager.UserInterface.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Security
{
    public class PermissionMaster
    {
        [Key]  
        [MaxLength(100)]
        public string PermissionGuid { get; set; }  
        [Required]
        [MaxLength(100)]
        public string ParentModule { get; set; }
        [Required]
        [MaxLength(100)]
        public string PermissionLevel { get; set; }
        [Required]
        [MaxLength(100)]
        public string PermissionShortName { get; set; }
        [Required]
        [MaxLength(500)]
        public string PermissionFullName { get; set; }

        public bool IsSelected { get; set; }
        public object PageClass { get; set; }

        public List<PermissionMaster> GetAllPermissions()
        {
            try
            {
                List<PermissionMaster> p = new List<PermissionMaster>
                {
                    //point of sale
                    new PermissionMaster() { PermissionGuid = "A1", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1", PageClass = new NewOrder() },
                    new PermissionMaster() { PermissionGuid = "A2", ParentModule = "A", PermissionShortName = "Check Out", PermissionFullName = "Checkout Ticket", PermissionLevel = "1", PageClass = new CheckoutTicket() },
                    new PermissionMaster() { PermissionGuid = "A3", ParentModule = "A", PermissionShortName = "Tickets", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new object() },
                    //products
                    new PermissionMaster() { PermissionGuid = "D1", ParentModule = "D", PermissionShortName = "Menu Categories", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuCategories() },
                    new PermissionMaster() { PermissionGuid = "D2", ParentModule = "D", PermissionShortName = "Menu Products", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuProducts() },
                   //settings
                    new PermissionMaster() { PermissionGuid = "F1", ParentModule = "F", PermissionShortName = "Tables", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                    new PermissionMaster() { PermissionGuid = "F2", ParentModule = "F", PermissionShortName = "Client Info", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                    new PermissionMaster() { PermissionGuid = "F3", ParentModule = "F", PermissionShortName = "Database", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                    //security
                    new PermissionMaster() { PermissionGuid = "G1", ParentModule = "G", PermissionShortName = "User Roles", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageRoles() },
                    new PermissionMaster() { PermissionGuid = "G2", ParentModule = "G", PermissionShortName = "System Users", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new ManageSystemUsers() }
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
