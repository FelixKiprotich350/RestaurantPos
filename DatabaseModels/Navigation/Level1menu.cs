using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels.Security;

namespace DatabaseModels.Navigation
{
    public class Level1menu
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<PermissionMaster> MenuItems { get; set; }

    }
}
