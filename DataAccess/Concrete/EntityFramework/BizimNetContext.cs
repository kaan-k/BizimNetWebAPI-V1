using Autofac.Core;
using Core.Entities.Concrete;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Customers;
using Entities.Concrete.Departments;
using Entities.Concrete.Devices;
using Entities.Concrete.DocumentFile;
using Entities.Concrete.Duties;
using Entities.Concrete.Employees;
using Entities.Concrete.InstallationRequests;
using Entities.Concrete.Offers;
using Entities.Concrete.Orders;
using Entities.Concrete.Payments;
using Entities.Concrete.Sections;
using Entities.Concrete.Services;
using Entities.Concrete.Settings;
using Entities.Concrete.Stocks;
using Entities.Concrete.Tables;
using Entities.Concrete.Warehouses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class BizimNetContext : DbContext
    {
        public BizimNetContext()
        {
        }
        public BizimNetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // FIX: Added .UseNpgsql() before the connection string
            optionsBuilder.UseNpgsql(@"Server=127.0.0.1;Port=5432;Database=BizimNetDB;User Id=postgres;Password=mysecretpassword;");
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Aggrement> Aggrements { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
        public DbSet<Duty> Duties{ get; set; }
        public DbSet<InstallationRequest> InstallationRequests { get; set; }
        public DbSet<Offer> Offers { get; set; }

        public DbSet<Billing> Billings { get; set; }
        public DbSet<Table> Tables{ get; set; }
        public DbSet<Section> Sections{ get; set; }
        public DbSet<StockGroup> StockGroups { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Servicing> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BusinessUser> BusinessUsers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<AgGridSettings> Settings { get; set; }
        public DbSet<OfferItem> OfferItems { get; set; } 




    }
}