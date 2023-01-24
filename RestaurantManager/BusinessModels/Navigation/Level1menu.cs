using RestaurantManager.BusinessModels.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RestaurantManager.BusinessModels.Navigation
{
    public class Level1menu
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }  
        public bool IsEnabled { get; set; }  
       // public SolidColorBrush BackgroundColor { get; set; }  

        public ObservableCollection<PermissionMaster> SubMenuItems { get; set; }
        public Level1menu()
        {
            SubMenuItems = new ObservableCollection<PermissionMaster>(); 
        }
    }
}
