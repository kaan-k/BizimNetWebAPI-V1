using Core.Configuration;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_EmployeeDal:MongoRepository<Employee>, IEmployeeDal
    {
        public Mongo_EmployeeDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
         : base(database, settings.Value.EmployeesCollectionName)
        {
        }
        public List<Employee> GetAll(Expression<Func<Employee, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Employee>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
