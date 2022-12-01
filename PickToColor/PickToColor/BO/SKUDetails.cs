using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickToColor.BO
{
    public class SKUDetails
    {
        public string SKU { get; set; }
        public string Description { get; set; }
        public int? ResumeItemIndex { get; set; }
        public int? PickingID { get; set; }
        public List<ContainingOrder> OrderItems { get; set; }

    }

    public class ContainingOrder
    {
        public string OrderCD { get; set; }
        public string Customer { get; set; }
        public decimal? Quantity { get; set; }
        public string LocationCode { get; set; }
        public string LocationSoundFile { get; set; }
        public string BackgroundColorCode { get; set; }
        public string FontColorCode { get; set; }
        public string ColorSoundFile { get; set; }
        public int? PickingPriority { get; set; }
        public bool IsItemReplenished { get; set; }
    }
}