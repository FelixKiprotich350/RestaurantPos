using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class StockFlowTransaction
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductGuid { get; set; }
        [Required] 
        public string ProductName { get; set; }
        [Required]
        [MaxLength(100)]
        public string FlowDirection { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]  
        public string OutTransactionCode { get; set; } 
        [Required]  
        public string InTransactionCode { get; set; } 
        [Required]  
        public string Description { get; set; } 
        [Required] 
        public DateTime TransactionDate { get; set; } 
        [Required] 
        public bool IsCancelled { get; set; }   

        /// <summary>
        /// Not mapped properties
        /// </summary>
        [NotMapped] 
        public bool IsSelected { get; set; }   
    }
}
