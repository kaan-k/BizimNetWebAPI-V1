using Core.DataAccess;
using Core.DataAccess.MongoDB;
using Core.Entities.Concrete; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBusinessUserDal:IMongoRepository<BusinessUser>
    {
        List<BusinessUser> GetAll();
    }
}
