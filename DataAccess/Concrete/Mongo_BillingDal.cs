using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Payment;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_BillingDal : MongoRepository<Billing>, IBillingDal
    {
        public Mongo_BillingDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
            : base(database, settings.Value.BillingsCollectionName)
        {
        }
    
    }
}
