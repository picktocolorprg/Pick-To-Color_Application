using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("ProductAllocation")]
    public class ProductAllocationModel
    {
        [Key]
        public int ProductAllocationID { get; set; }
        public int ProductID { get; set; }
        public int PickingPriority { get; set; }
        public int LocationID { get; set; }
    }
}