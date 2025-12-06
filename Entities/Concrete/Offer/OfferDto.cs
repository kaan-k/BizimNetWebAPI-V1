using Core.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace Entities.Concrete.Offers // ✅ Changed to PLURAL
{
    public class OfferDto : IDto
    {
        public int Id { get; set; } // Added Id
        public int CustomerId { get; set; } // ✅ int
        public string OfferTitle { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; } // ✅ decimal

        public List<OfferItemDto> Items { get; set; } // List of DTOs is fine here

        public DateTime ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

   
}