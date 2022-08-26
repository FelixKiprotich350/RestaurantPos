using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Navigation
{
    class NavigationMenu
    {
        public List<Level1menu> MenuCategories { get; set; }
        public NavigationMenu()
        {
            MenuCategories = new List<Level1menu>
            { 
                new Level1menu() { GroupName = "Point of Sale", GroupCode = "A", GroupIcon = "AccountStudent" },
                new Level1menu() { GroupName = "Accounts", GroupCode = "B", GroupIcon = "Book" },
                new Level1menu() { GroupName = "Products", GroupCode = "C", GroupIcon = "CurrencyUsd" },
                new Level1menu() { GroupName = "Reports", GroupCode = "D", GroupIcon = "Hotel" },
                new Level1menu() { GroupName = "Settings", GroupCode = "E", GroupIcon = "People" }
            };
        }
    }
}
