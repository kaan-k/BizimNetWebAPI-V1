﻿using Core.DataAccess.MongoDB;
using Entities.Concrete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IServicingDal:IMongoRepository<Servicing>
    {
        List<Servicing> GetAllServicingDetails();
    }
}
