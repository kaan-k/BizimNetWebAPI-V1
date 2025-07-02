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
        public string DeviceCollectionName { get; set; } = "Devices";

        public string InstallationRequestsCollectionName { get; set; } = "InstallationRequests";

        public string BusinessUserCollectionName { get; set; } = "BusinessUsers";
        public string EmployeesCollectionName { get; set; } = "Employees";
        public string DocumentsCollectionName { get; set; } = "Documents";
        public string OffersCollectionName { get; set; } = "Offers";
        public string ServicesCollectionName { get; set; } = "Services";


        public string DepartmentsCollectionName { get; set; } = "Departments";
        public string StatusRecordCollectionName { get; set; } = "StatusRecords";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "BizimNetDB";
    }
}
