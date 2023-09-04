using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels.Navigation;

namespace DatabaseModels.Navigation
{
    public class NavigationMenu
    {
        public List<Level1menu> MenuCategories = new List<Level1menu>();
        public NavigationMenu()
        {
            List<Level1menu> final = new List<Level1menu>();
            MenuCategories.Add(new Level1menu() { GroupCode = "A", GroupName="Sales Point", IsEnabled = true });
            MenuCategories.Add(new Level1menu() { GroupCode = "B", GroupName="Work period", IsEnabled = true });
            MenuCategories.Add(new Level1menu() { GroupCode = "C", GroupName="Accounts", IsEnabled = true });
            MenuCategories.Add(new Level1menu() { GroupCode = "D", GroupName="Warehouse", IsEnabled = true });
            MenuCategories.Add(new Level1menu() { GroupCode = "E", GroupName="Reports", IsEnabled = true });
            MenuCategories.Add(new Level1menu() { GroupCode = "F", GroupName="Settings", IsEnabled = true }); 
        }
        
    }
}
