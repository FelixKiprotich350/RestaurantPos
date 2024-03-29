﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace DatabaseModels.Security
{
    public class PosUser
    {
        [Key]
        [MaxLength(100)] 
        public string UserGuid { get; set; }
        [Index]
        [Required]  
        public int UserPIN { get; set; }
        [Required]
        [Index]
        [MaxLength(100)] 
        public string UserName { get; set; }

        [Required] 
        public string UserFullName { get; set; }
        [Required] 
        public bool IsDefaultpin { get; set; }
        [Required] 
        public bool IsPosUser { get; set; }
        [Required] 
        public bool IsBackendUser { get; set; }
        [Required] 
        [MaxLength(100)] 
        public string PhoneNumber { get; set; }
        [Required] 
        [MaxLength(100)]
        public string UserRole { get; set; }
        [Required]
        [MaxLength(100)] 
        public string UserWorkingStatus { get; set; }
        [Required] 
        public string UserRights { get; set; }
        [Required]  
        public bool UserIsDeleted { get; set; } 
        [Required] 
        public DateTime RegistrationDate { get; set; }
        [Required] 
        public DateTime LastLoginDate { get; set; } 
        [NotMapped]
        public List<PermissionMaster> User_Permissions_final { get; set; }
    }
}
