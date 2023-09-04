using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels.Navigation;

namespace DatabaseModels.Security
{
    public class UserRole
    {
        [Key]
        [MaxLength(100)]
        public string RoleGuid { get; set; } 
        [Required]
        [Index("RoleName", -1, IsUnique = true)]
        [MaxLength(100)]
        public string RoleName { get; set; } 
        [Required]
        [MaxLength(500)]
        public string RoleDescription { get; set; }
        [Required]
        [MaxLength(100)] 
        public string RoleStatus { get; set; }
        [Required]
        [MaxLength(100)]
        public string RolePermissions { get; set; }
        [Required]
        [MaxLength(100)]
        public string RoleIsDeleted { get; set; }
        [Required] 
        public DateTime RegistrationDate { get; set; }
        [Required] 
        public DateTime LastUpdateDate { get; set; } 
        [NotMapped]
        public List<PermissionMaster> User_Permissions_final { get; set; }
    }
}
