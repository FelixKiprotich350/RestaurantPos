using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantPos.BusinessModels.Navigation;

namespace RestaurantPos.BusinessModels.Security
{
    public class ERPMasterRoles
    {
        public ERPMasterRoles()
        {
            MenuCategoriesList.Clear();
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Order Tickets", GroupCode = "A",GroupIcon= "Administrator" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Point of Sale", GroupCode = "B", GroupIcon = "AccountStudent" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Accounts", GroupCode = "C", GroupIcon = "Book" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Products", GroupCode = "D", GroupIcon = "CurrencyUsd" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Customers", GroupCode = "E", GroupIcon = "Hotel" });
            MenuCategoriesList.Add(new Level1menu() { GroupName = "Settings", GroupCode = "F", GroupIcon = "People" }); 
        }
        readonly public List<Level1menu> MenuCategoriesList = new List<Level1menu>();
    }
}
