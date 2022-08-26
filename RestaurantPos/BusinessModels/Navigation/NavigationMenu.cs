using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPos.BusinessModels.Navigation
{
    class NavigationMenu
    {
        public List<Level1menu> MenuCategories { get; set; }
        public NavigationMenu()
        {
            MenuCategories = new List<Level1menu>();
        }
    }
}
