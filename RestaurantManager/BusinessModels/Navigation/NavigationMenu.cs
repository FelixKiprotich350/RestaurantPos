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
                new Level1menu() { GroupName = "Point of Sale", GroupCode = "A" },
                new Level1menu() { GroupName = "Work Periods", GroupCode = "B" },
                new Level1menu() { GroupName = "Accounts", GroupCode = "C"  },
                new Level1menu() { GroupName = "WareHouse", GroupCode = "D"  },
                new Level1menu() { GroupName = "Reports", GroupCode = "E"  },
                new Level1menu() { GroupName = "Settings", GroupCode = "F" },
               // new Level1menu() { GroupName = "Security", GroupCode = "G" },
                new Level1menu() { GroupName = "Customers", GroupCode = "H" }
            };
        }
    }
}
