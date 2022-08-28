using RestaurantManager.UserInterface.MenuProducts;
using RestaurantManager.UserInterface.PointofSale;
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
                List<PermissionMaster> p = new List<PermissionMaster>();
                p.Add(new PermissionMaster() { PermissionGuid = "A1", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1", PageClass = new NewOrder() });
                p.Add(new PermissionMaster() { PermissionGuid = "A2", ParentModule = "A", PermissionShortName = "Check Out", PermissionFullName = "Checkout Ticket", PermissionLevel = "1", PageClass = new CheckoutTicket() });
                p.Add(new PermissionMaster() { PermissionGuid = "A3", ParentModule = "A", PermissionShortName = "Tickets", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new object() });
                p.Add(new PermissionMaster() { PermissionGuid = "D1", ParentModule = "C", PermissionShortName = "Menu Categories", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuCategories() });
                p.Add(new PermissionMaster() { PermissionGuid = "D2", ParentModule = "C", PermissionShortName = "Menu Products", PermissionFullName = "Description", PermissionLevel = "1", PageClass = new MenuProducts() });
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
