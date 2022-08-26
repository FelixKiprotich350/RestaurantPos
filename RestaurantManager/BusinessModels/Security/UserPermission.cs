using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Security
{
    public class UserPermission
    {
        [Key]  
        public int PermissionID { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserGuid { get; set; }
        [Required]
        [MaxLength(100)]
        public string ParentPermissionGuid { get; set; }

    }
}
