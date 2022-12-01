using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickToColor.EntityFramework;
using PickToColor.Models;

namespace PickToColor.Utility
{
    public class TransactionAudit
    {

        public DataContext Context;

        public TransactionAudit()
        {
            Context = new DataContext();
        }

        public enum AuditType
        {
            Login = 1,
            Users = 2,
            Customers = 3,
            Stations = 4,
            Locations = 5,
            Boxes = 6,
            SingleOrderPicking = 7,
            Report = 8
        }

        public void AddHistory(AuditType TypeOfAudit, int AuditTypeID, string Description)
        {
            TransactionAuditModel AuditItem = new TransactionAuditModel();
            AuditItem.AuditType = (int)TypeOfAudit;
            AuditItem.AuditTypeID = AuditTypeID;
            AuditItem.CreatedOn = DateTime.Now;
            AuditItem.TransactionDescription = Description;
            AuditItem.UserID = (HttpContext.Current.Session[KeyConstants.CurrentUser] as UserModel) != null ? (HttpContext.Current.Session[KeyConstants.CurrentUser] as UserModel).UserID : 0;

            Context.TransactionAudits.Add(AuditItem);
            Context.SaveChanges();
        }
    }
}