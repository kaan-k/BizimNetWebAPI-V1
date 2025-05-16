using Core.DataAccess.MongoDB;
using Entities.Concrete.InstallationRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IInstallationRequestDal:IMongoRepository<InstallationRequest>
    {
    }
}
