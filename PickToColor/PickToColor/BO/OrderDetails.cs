using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickToColor.Models;

namespace PickToColor.BO
{
    public class OrderDetails

    {
        public string OrderCD { get; set; }
        public string OrderType { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool? OneScanningEnabled { get; set; }
        public int? BoxTypeID { get; set; }
        public string BoxTypeName { get; set; }
        public bool AllItemsInCurrentStation { get; set; }
        public int TotalCartItem { get; set; }
        public int? PickingID { get; set; }
        public int? ResumeItemIndex { get; set; }
        public List<OrderItems> OrderedItems;
        public List<string> WarehouseSKUs;
        public int? TotalQuantity { get; set; }
    }

    public class OrderItems
    {
        public int? ProductID { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public decimal AssociatedPickingQty { get; set; }
        public bool? CheckSN { get; set; }
        public decimal? CBM { get; set; }
        public int? LocationMasterID { get; set; }
        public string LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationSoundFile { get; set; }
        public string BackgroundColorCode { get; set; }
        public string FontColorCode { get; set; }
        public string ColorSoundFile { get; set; }
        public int? PickingPriority { get; set; }
        public decimal? OrderQty { get; set; }
        public bool IsItemPicked { get; set; }
    }
}