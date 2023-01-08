using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels
{
    public class TestModel
    {
        [Key]
        public string testkey { get; set; }
    }
}
