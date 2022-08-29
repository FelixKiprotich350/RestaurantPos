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
        [Column(Order =0)]
        public string UserGuid { get; set; }
        [Index]
        [Required]
        [Column(Order = 1)]
        public int UserPIN { get; set; }
        [Required]
        [MaxLength(500)]
        [Column(Order = 2)]
        public string UserFullName { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 3)]
        public string UserRole { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(Order = 4)]
        public string UserWorkingStatus { get; set; }
        [Required] 
        [Column(Order = 5)]
        public bool UserIsDeleted { get; set; }
        [Required]
        [MaxLength(500)]
        [Column(Order = 6)]
        public string UserRights { get; set; }
        [Required]
        [Column(Order = 7)]
        public DateTime RegistrationDate { get; set; }
        [Required]
        [Column(Order = 8)]
        public DateTime LastLoginDate { get; set; } 
        [NotMapped]
        public List<PermissionMaster> User_Permissions_final { get; set; }
    }
}
