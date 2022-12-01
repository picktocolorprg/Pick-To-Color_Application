using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PickToColor.Models;

namespace PickToColor.EntityFramework
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<LocationModel> Locations { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<BoxesModel> Boxes { get; set; }

        public DbSet<OrderModel> Orders { get; set; }

        public DbSet<OrderTypesModel> OrderTypes { get; set; }

        public DbSet<StationModel> Stations { get; set; }

        public DbSet<ProductPickingModel> ProductPickings { get; set; }

        public DbSet<ProductPickingItemModel> ProductPickingLineItems { get; set; }

        public DbSet<TransactionAuditModel> TransactionAudits { get; set; }

      
    }
}