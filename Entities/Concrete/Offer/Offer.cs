using Core.Entities.Abstract;
using Entities.Concrete.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Offers
{
    public class Offer : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        public string OfferTitle { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Description { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public virtual ICollection<OfferItem> Items { get; set; } = new List<OfferItem>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
