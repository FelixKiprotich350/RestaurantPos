using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class StockTakingMaster
    {
        //constructors

        //properties
        [Key]
        [MaxLength(100)]
        public string ItemGuid { get; set; }
        [Required]
        [MaxLength(500)]
        public string STTNumber{ get; set; }
        [Required] 
        public string Notes { get; set; }  
        [Required]
        public DateTime OpeningDate { get; set; } 
        [Required]
        public DateTime ClosingDate { get; set; }
        [Required]
        [MaxLength(200)]
        public string OpenedBy { get; set; }
        [Required]
        [MaxLength(200)]
        public string ClosedBy { get; set; }
        [Required]
        [MaxLength(200)]
        public string Status { get; set; }
    }
}
