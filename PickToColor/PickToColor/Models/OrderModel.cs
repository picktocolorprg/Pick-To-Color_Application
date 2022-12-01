using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Orders")]
    public class OrderModel
    {
        [Key]
        [Column(Order =0)]
        public int CustomerID { get; set; }
        [Key]
        [Column(Order = 1)]
        public string OrderCD { get; set; }
        [Key]
        [Column(Order = 2)]
        public string SKU { get; set; }
        public decimal Quantity { get; set; }
        public int OrderTypeID { get; set; }
        public bool OrderCompletedStatus { get; set; }
}

    /// <summary>
    /// This enum should reflect ordertype table
    /// </summary>
    public enum OrderType
    {
        SinglePick = 1,
        
    }
}