using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.BO;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    public class SingleOrderPickingController : Controller
    {
        public DataContext context;

        public SingleOrderPickingController()
        {
            context = new DataContext();
        }

        // GET: SingleOrderPicking
        [StationSet]
        public ActionResult Index()
        {
            return View();
        }

        [CustomException]
        [HttpPost]
        public void CheckForCancelledPicking(string OrderCD, int StationID)
        {
            int CancelledPicking = context.ProductPickings.Count(order => order.PickStatus == (int)PickingStatus.Cancelled && order.OrderCD == OrderCD);
            int TotalPicking = context.ProductPickings.Count(order => order.OrderCD == OrderCD);
            //If the picking is cancelled before and no new picking is initialised yet.
            if (TotalPicking != 0 && CancelledPicking == TotalPicking)
            {
                var OrderCDParam = new SqlParameter("@ORDER_CD", OrderCD);
                var StationIDParam = new SqlParameter("@STATION_ID", StationID);
                //Call spGetOrderDetails SP to get the order details
                OrderDetails CurrentOrder = context.Database.SqlQuery<OrderDetails>("spGetOrderDetails @ORDER_CD, @STATION_ID", OrderCDParam, StationIDParam).Single();
                context.ProductPickings.Add(new ProductPickingModel()
                {
                    BoxTypeID = CurrentOrder.BoxTypeID.HasValue ? CurrentOrder.BoxTypeID.Value : 0,
                    OperatorUserName = HttpContext.User.Identity.Name,
                    OrderCD = OrderCD,
                    OrderType = (int)OrderType.SinglePick,
                    PickingStartTime = DateTime.Now,
                    PickStatus = (int)PickingStatus.StartedPicking,
                });
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns the scanned order detail in form of a JSON object
        /// Validations:
        /// 1. Check if the scanned order exists and is a Single Pick Order
        /// 2. Check if all the items belong to the particular station the user is currently working on
        /// </summary>
        /// <param name="OrderCD"></param>
        /// <returns></returns>
        [CustomException]
        [HttpPost]
        public string GetOrderDetails(string OrderCD, int StationID)
        {
            if (!string.IsNullOrEmpty(OrderCD))
            {
                OrderDetails CurrentOrder = null;
                
                if (context.Orders.Any(a => a.OrderCD == OrderCD && a.OrderTypeID == (int)OrderType.SinglePick))
                {
                    var OrderCDParam = new SqlParameter("@ORDER_CD", OrderCD);
                    var StationIDParam = new SqlParameter("@STATION_ID", StationID);
                    //Call spGetOrderDetails SP to get the order details
                    CurrentOrder = context.Database.SqlQuery<OrderDetails>("spGetOrderDetails @ORDER_CD, @STATION_ID", OrderCDParam, StationIDParam).Single();
                    ValidateOrder(OrderCD, CurrentOrder);

                    CurrentOrder.OrderedItems = new List<OrderItems>();
                    OrderCDParam = new SqlParameter("@ORDER_CD", OrderCD);
                    StationIDParam = new SqlParameter("@STATION_ID", StationID);
                    CurrentOrder.OrderedItems = context.Database.SqlQuery<OrderItems>("spGetOrderItemDetails @ORDER_CD,@STATION_ID", OrderCDParam, StationIDParam).ToList();
                    if (CurrentOrder.OrderedItems != null)
                    {
                        if (CurrentOrder.OrderedItems.Count > 0)
                        {
                            CurrentOrder.OrderedItems.ForEach(item =>
                            {
                                item.ColorSoundFile = Url.Content("~/" + item.ColorSoundFile);
                                item.LocationSoundFile = Url.Content("~/" + item.LocationSoundFile);
                            });
                        }
                        else
                        {
                            throw new Exception(string.Format("Order# {0} contains SKUs that are not available in your station", OrderCD));
                            return null;
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Order# {0} contains SKUs that are not available in your station", OrderCD));
                        return null;
                    }
                    CurrentOrder.TotalCartItem = CurrentOrder.OrderedItems != null ? CurrentOrder.OrderedItems.Where(a => a.LocationMasterID != null && a.LocationMasterID.HasValue).Count() : 0;
                    //If no item is picked, Start from 0, otherwise start from item which is not Picked yet
                    CurrentOrder.ResumeItemIndex = CurrentOrder.OrderedItems.Any(a => !a.IsItemPicked) ? CurrentOrder.OrderedItems.FindIndex(a => !a.IsItemPicked) : 0;
                    //TODO : Clear the midway scanned lineitems for the resuming SKU in DB
                    if(CurrentOrder.OrderType == "MIX")
                    {
                        OrderCDParam = new SqlParameter("@ORDER_CD", OrderCD);
                        CurrentOrder.WarehouseSKUs = context.Database.SqlQuery<string>("spGetWarehouseSKUs @ORDER_CD", OrderCDParam).ToList();
                    }

                    return Newtonsoft.Json.JsonConvert.SerializeObject(CurrentOrder);
                }
                else
                {
                    throw new Exception("The scanned order number either does not exist or is not a Single Pick Order. Please check and try again.");
                }
            }
            return null;
        }


        private void ValidateOrder(string OrderCD, OrderDetails CurrentOrder)
        {
            if (!CurrentOrder.AllItemsInCurrentStation)
                throw new Exception(string.Format("Order# {0} contains SKUs that are not available in your station", OrderCD));

            //if (string.IsNullOrEmpty(CurrentOrder.BoxTypeName))
            //    throw new Exception("There is no suitable Box type available to fullfill this order.");

            if (CurrentOrder.OrderType == "WH")
                throw new Exception("This is a Warehouse order. No item is available at the picking station to fulfill this order.");


            //if (CurrentOrder.PickingID.HasValue)
            //{
                //if (context.ProductPickings.Any(a => a.PickingID == CurrentOrder.PickingID.Value && a.PickStatus == (int)PickingStatus.PickingCompleted))
                //{
                //    throw new Exception("The order is already picked. Please scan other order number or visit the reports section to view details about the scanned order.");
                //}
                //if (context.Orders.Any(a => a.OrderCD == CurrentOrder.OrderCD && a.OrderCompletedStatus))                
                //{
                //    throw new Exception("The order is already picked. Please scan other order number or visit the reports section to view details about the scanned order.");
                //}
            //}
        }

        [HttpPost]
        [CustomException]
        public int StartOrderPicking(string OrderCD, int BoxTypeID, DateTime? PickingStartTime)
        {
            ProductPickingModel picking = new ProductPickingModel();
            picking.BoxTypeID = BoxTypeID;
            picking.OperatorUserName = HttpContext.User.Identity.Name;
            picking.OrderCD = OrderCD;
            picking.OrderType = (int)OrderType.SinglePick;
            picking.PickingStartTime = PickingStartTime.Value;
            picking.PickStatus = (int)PickingStatus.StartedPicking;

            context.ProductPickings.Add(picking);
            context.SaveChanges();

            return picking.PickingID;

        }


        [CustomException]
        [HttpPost]
        public void SavePickingItem(int PickingID, int ProductID, string SerialNo, DateTime ScannedOn)
        {
            try
            {
                context.ProductPickingLineItems.Add(new ProductPickingItemModel()
                {
                    PickingID = PickingID,
                    ProductID = ProductID,
                    SerialNo = SerialNo,
                    ScannedOn = ScannedOn
                });
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && (ex.InnerException.InnerException as SqlException) != null && (ex.InnerException.InnerException as SqlException).Number == 2627)  //Check for Primary Key Violation
                {
                    throw new Exception("The item with the serial number value " + SerialNo + " is already scanned");
                }
                else
                    throw ex;
            }
            catch (Exception exx)
            {
                throw exx;
            }
        }

        [HttpPost]
        [CustomException]
        public void PickingComplete(int PickingID, DateTime PickingEndTime, string OrderCD)
        {
            var CurrentOrderPicking = context.ProductPickings.FirstOrDefault(existingOrder => existingOrder.PickingID == PickingID);
            if (CurrentOrderPicking == null)
                return;

            CurrentOrderPicking.PickStatus = (int)PickingStatus.PickingCompleted;
            CurrentOrderPicking.PickingEndTime = PickingEndTime;
            context.SaveChanges();

            var ordercompleted = context.Orders.Where(a => a.OrderCD.Contains(OrderCD)).ToList();
            ordercompleted.ForEach(a => a.OrderCompletedStatus = true);
            //OrderModel EditOrderStatus = context.Orders.Where(a => a.OrderCD == OrderCD);
            //EditOrderStatus.OrderCompletedStatus = true;            
            context.SaveChanges();
            
        }

        [HttpPost]
        [CustomException]
        public void CancelPicking(int PickingID)
        {
            var Picking = context.ProductPickings.FirstOrDefault(a => a.PickingID == PickingID);
            Picking.PickStatus = (int)PickingStatus.Cancelled;
            Picking.PickingEndTime = DateTime.Now;
            context.SaveChanges();
        }

    }
}