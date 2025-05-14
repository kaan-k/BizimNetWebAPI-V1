using Core.DataAccess.MongoDB;
using Core.Entities.Concrete;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IEmployeeDal:IMongoRepository<Employee>
    {

    }
}
