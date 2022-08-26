using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantPos.BusinessModels.Administration
{
    public class MyInstitution
    {
        public string InstitutionName { get; set; }
        public string InstitutionPhysicalAddress { get; set; }
        public string InstitutionEmail { get; set; }
        public static string InstitutionBranch { get; set; } 
        public static string InstitutionTel { get; set; }
    }
}
