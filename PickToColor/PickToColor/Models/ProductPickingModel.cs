using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("ProductPicking")]
    public class ProductPickingModel
    {
        [Key]
        public int PickingID { get; set; }
        public string OrderCD { get; set; }
        public string OperatorUserName { get; set; }
        public DateTime? PickingStartTime { get; set; }
        public DateTime? PickingEndTime { get; set; }
        public int OrderType { get; set; }
        public int BoxTypeID { get; set; }
        public int PickStatus { get; set; }
        [NotMapped]
        public PickingStatus PickingStatus { get { return (PickingStatus)this.PickStatus; } }
    }

    public enum PickingStatus
    {
        Unpicked = 0,
        StartedPicking = 1,
        PickingCompleted = 2,
        Cancelled = 3
    }
}