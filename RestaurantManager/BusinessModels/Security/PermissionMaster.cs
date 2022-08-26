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
                p.Add(new PermissionMaster() { PermissionGuid = "A3", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1" });
                p.Add(new PermissionMaster() { PermissionGuid = "A4", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1" });
                p.Add(new PermissionMaster() { PermissionGuid = "A5", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1" });
                p.Add(new PermissionMaster() { PermissionGuid = "A6", ParentModule = "A", PermissionShortName = "New Order", PermissionFullName = "Make a new order", PermissionLevel = "1" });
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
