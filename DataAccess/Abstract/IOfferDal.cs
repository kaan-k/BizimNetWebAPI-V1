using Core.DataAccess.MongoDB;
using DataAccess.Repositories;
using Entities.Concrete.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IOfferDal:IMongoRepository<Offer>
    {
        public List<Offer> GetAllOfferDetails();
        public List<Offer> GetByStatus(string status);

    }
}
