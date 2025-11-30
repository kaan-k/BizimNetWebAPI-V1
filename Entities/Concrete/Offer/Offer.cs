using Core.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Entities.Concrete.Offer
{
    public class Offer : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string OfferTitle { get; set; }
        public string Status { get; set; } = "Pending";
        public string Description{ get; set; }
        public int TotalAmount { get; set; }
        public List<OfferItemDto> items { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

}
