using RestaurantManager.BusinessModels.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Navigation
{
    public class Level1menu
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; } 
        public string GroupIcon { get; set; } 

        public ObservableCollection<PermissionMaster> SubMenuItems { get; set; }
        public Level1menu()
        {
            SubMenuItems = new ObservableCollection<PermissionMaster>(); 
        }
    }
}
