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
        [BsonRepresentation(BsonType.ObjectId)]
        public string OfferId { get; set; }

        public string AggrementTitle{ get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        public string AggrementType { get; set; }
        public int AgreedAmount { get; set; }// Match Offer.TotalAmount
        public int PaidAmount{ get; set; }

        public List<string> billings { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public bool isActive { get; set; }

    }
}
