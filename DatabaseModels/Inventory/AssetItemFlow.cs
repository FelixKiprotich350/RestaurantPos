using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Inventory
{
    public class AssetItemFlow
    {
        [Key]
        [MaxLength(100)]
        public string AssetFlowGuid { get; set; }
        [Required]
        [MaxLength(200)]
        public string AssetItemGuid { get; set; }
        [Required]
        public string AssetName { get; set; }
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
