using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantPos.BusinessModels.Navigation;

namespace RestaurantPos.BusinessModels.Security
{
    public class ERPUser
    {
        public ERPUser()
        {
            UserName = null;
            UserFullName = null;
            UserStatus = null;
            UserRoles.Clear();
        }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserStatus { get; set; } 

        public List<Level2menu> UserRoles = new List<Level2menu>();
    }
}
