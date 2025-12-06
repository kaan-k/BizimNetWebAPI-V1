using Core.Entities.Abstract;
using Entities.Concrete.Offers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Offer
{
    public class OfferDetailsDto : IDto
    {
        public int Id { get; set; } // Added Id
        public int CustomerId { get; set; } // ✅ int
        public string OfferTitle { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; } // ✅ decimal

        public List<OfferItemDto> Items { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

