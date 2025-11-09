using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Aggrements
{
    
    public class Aggrement : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }

        public string AggrementType { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool isActive { get; set; }

    }
}
