using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManager.BusinessModels.Menu
{
    class ProductCategory
    { 
        //constructors
         
        //properties
        [Key]
        [MaxLength(100)]
        public string CategoryGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; }
        [Required] 
        public DateTime CreationDate { get; set; }
        [NotMapped]
        public List<MenuProductItem> MenuItems { get; set; }

        public void GetMenuItems(string catid)
        {
            try
            {
                using (var db=new PosDbContext())
                {
                    var a = db.MenuProductItem.ToList().Where(t=>t.CategoryGuid==catid).ToList();
                    this.MenuItems = a;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
