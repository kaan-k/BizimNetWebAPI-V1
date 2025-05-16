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
        public string InstallationRequestsCollectionName { get; set; } = "InstallationRequests";

        public string BusinessUserCollectionName { get; set; } = "BusinessUsers";
        public string EmployeesCollectionName { get; set; } = "Employees";
        public string DocumentsCollectionName { get; set; } = "Documents";
        public string OffersCollectionName { get; set; } = "Offers";

        public string DepartmentsCollectionName { get; set; } = "Departments";
        public string StatusRecordCollectionName { get; set; } = "StatusRecords";
    }
}
