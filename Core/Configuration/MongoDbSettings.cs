using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public class MongoDbSettings
    {
        public string CustomerCollectionName { get; set; } = "Customers";
        public string RestaurantCollectionName { get; set; } = "Restaurants";
        public string DepartmentsCollectionName { get; set; } = "Departments";
        public string DeviceCollectionName { get; set; } = "Devices";
        public string DocumentFileCollectionName { get; set; } = "DocumentFile";


        public string MenuItemCollectionName { get; set; } = "MenuItems";

        public string BusinessUserCollectionName { get; set; } = "BusinessUsers";
        public string StockCollectionName { get; set; } = "Stocks";
        public string EmployeesCollectionName { get; set; } = "Employees";

        public string InstallationRequestsCollectionName { get; set; } = "InstallationRequest";
        public string OffersCollectionName { get; set; } = "Offers";

        public string ServicesCollectionName { get; set; } = "Servicing";

        
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "QRMenuDB";
        public string MenuCategoryCollectionName { get; set; } = "MenuCategories";
    }
}
