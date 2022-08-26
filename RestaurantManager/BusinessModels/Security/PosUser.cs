using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManager.BusinessModels.Navigation;

namespace RestaurantManager.BusinessModels.Security
{
    public class PosUser
    {
        [Key]
        [MaxLength(100)]
        public string UserGuid { get; set; }
        [Required] 
        public int Username { get; set; }
        [Required]
        [MaxLength(500)]
        public string UserFullName { get; set; }
        [Required] 
        public int UserPin { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserStatus { get; set; }
        [NotMapped]
        public List<UserPermission> User_Permissions_raw { get; set; }
        [NotMapped]
        public List<PermissionMaster> User_Permissions_final { get; set; }
    }
}
