using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("OrderTypes")]
    public class OrderTypesModel
    {
        [Key]
        public int OrderTypeID { get; set; }
        public string OrderTypeName { get; set; }
    }
}