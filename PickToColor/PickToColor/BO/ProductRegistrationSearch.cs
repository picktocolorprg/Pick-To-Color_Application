using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickToColor.BO
{
    public class ProductRegistrationSearch
    {
        public string SKU { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public int? CustomerID { get; set; }
    }

 
}