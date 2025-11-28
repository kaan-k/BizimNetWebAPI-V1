using Core.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Offer
{
    public class OfferDetailsDto:IDto
    {
        public string CustomerId { get; set; }
        public string OfferTitle { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OfferItemDto> items { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

