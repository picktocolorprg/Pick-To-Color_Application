using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickToColor.BO
{
    public class Boxes
    {
        public int BoxTypeID { get; set; }
        public string BoxTypeCode { get; set; }
        public string BoxTypeDescription { get; set; }
        public decimal TotalCBM { get; set; }
        public Int32 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}