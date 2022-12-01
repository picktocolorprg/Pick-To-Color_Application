using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("ProductPickingLineItems")]
    public class ProductPickingItemModel
    {
        [Required]
        public int PickingID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public string SerialNo { get; set; }
        [Required]
        public DateTime ScannedOn { get; set; }
        [Key]
        public int ProductPickingLineItemID { get; set; }
    }
}