using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    public class StationController : Controller
    {
        public DataContext context;
        public int PageSize;

        public StationController()
        {
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
            context = new DataContext();
        }


        // GET: Station
        public ActionResult Index()
        {
            return View();
        }

        [CustomException]
        public ActionResult Create(string StationName)
        {
            if(!string.IsNullOrEmpty(StationName))
            {
                if (context.Stations.Any(a => a.StationName == StationName && a.IsActive))
                {
                    throw new Exception("Station name already exists.");
                }

                StationModel station = new StationModel();
                station.StationName = StationName;
                station.CreatedBy = HttpContext.User.Identity.Name;
                station.CreatedDate = DateTime.Now;
                station.IsActive = true;
                context.Stations.Add(station);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Station Name is empty.");
            }
            return new EmptyResult();
        }

        public ActionResult GetAllStations(int? pageno)
        {
            int pagenumber = (pageno ?? 1);
            PagedList.IPagedList<StationModel> Stations = null;
            Stations = context.Stations.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate).ToList().ToPagedList(pagenumber, PageSize);
            return PartialView(Stations);
        }

        [CustomException]
        public ActionResult Edit(int StationId, string StationName)
        {
            if (context.Stations.Any(existingStation => existingStation.StationName == StationName && existingStation.IsActive && existingStation.StationID != StationId))
                throw new Exception("The Station Name " + StationName + " already exists. Please try another name");

            StationModel EditStationDetail = context.Stations.Single(a => a.StationID== StationId);
            EditStationDetail.StationName = StationName;
            context.SaveChanges();
            return (new EmptyResult());
        }

        public ActionResult SelectStation(int? pageno)
        {
            int pagenumber = (pageno ?? 1);
            PagedList.IPagedList<StationModel> Stations = null;
            Stations = context.Stations.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate).ToList().ToPagedList(pagenumber, PageSize);
            return View(Stations);
        }

        [HttpPost]
        public bool SelectStation(int StationID)
        {
            try
            {
                StationModel CurrentStation = context.Stations.First(existingStation => existingStation.StationID == StationID);
                Session[KeyConstants.CurrentStation] = CurrentStation;
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Delete(List<string> StationIDs)
        {
            context.Stations.Where(a => StationIDs.Contains(a.StationID.ToString())).ToList().ForEach(a =>
            {
                a.IsActive = false;
            });
            context.SaveChanges();
            return (new EmptyResult());
        }
    }
}