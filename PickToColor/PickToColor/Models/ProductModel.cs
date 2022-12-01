using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Products")]
    public class ProductModel
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Barcode { get; set; }
        [Required]
        [MaxLength(20)]
        public string SKU { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        [Required]
        public decimal AssociatedPickingQty { get; set; }
        [Required]
        public bool CheckSN { get; set; }
        [Required]
        public decimal CBM { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public int LocationMasterID { get; set; }
    }
}