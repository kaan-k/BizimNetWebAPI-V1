using Core.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace Entities.Concrete.Offers
{
    public class OfferDto : IDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public string OfferTitle { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }

        public DateTime? ExpirationDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OfferItemDto> Items { get; set; } = new();
    }
}
