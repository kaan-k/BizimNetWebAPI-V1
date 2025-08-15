using Core.Configuration;
using DataAccess.Abstract;
using DataAccess.Repositories;
using Entities.Concrete.InviteToken;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class Mongo_InviteTokenDal:MongoRepository<InviteToken>, IInviteTokenDal
    {
        private readonly IMongoDatabase _db;
        public Mongo_InviteTokenDal(IMongoDatabase database, IOptions<MongoDbSettings> settings)
          : base(database, settings.Value.InvitationTokenCollectionName)
        {
            _db = database;
        }
    }
}
