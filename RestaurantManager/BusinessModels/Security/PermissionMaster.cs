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
        public object PageClass { get; set; }

        public List<PermissionMaster> GetAllPermissions()
        {
            try
            {
                List<PermissionMaster> p = new List<PermissionMaster>
                {
                    new PermissionMaster() { PermissionGuid = "A1", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1", PageClass = new NewOrder() },
                    new PermissionMaster() { PermissionGuid = "A2", ParentModule = "A", PermissionShortName = "Check Out", PermissionFullName = "Checkout Ticket", PermissionLevel = "1", PageClass = new CheckoutTicket() },
                    new PermissionMaster() { PermissionGuid = "A3", ParentModule = "A", PermissionShortName = "Tickets", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new object() },
                    new PermissionMaster() { PermissionGuid = "D1", ParentModule = "C", PermissionShortName = "Menu Categories", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuCategories() },
                    new PermissionMaster() { PermissionGuid = "D2", ParentModule = "C", PermissionShortName = "Menu Products", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuProducts() },
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
