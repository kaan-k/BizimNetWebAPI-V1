using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Customer;
using Entities.Concrete.Department;
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
    public class Mongo_DepartmentDal : MongoRepository<Department>,IDepartmentDal
    {
        public Mongo_DepartmentDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
          : base(database, settings.Value.DepartmentsCollectionName)
        {
        }
        public List<Department> GetAll(Expression<Func<Department, bool>> filter = null)
        {
            if (filter == null)
                return _collection.Find(FilterDefinition<Department>.Empty).ToList();

            return _collection.Find(filter).ToList();
        }
    }
}
