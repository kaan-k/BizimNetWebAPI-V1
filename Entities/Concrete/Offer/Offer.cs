using Core.Entities.Abstract;
using Entities.Concrete.Customers; // ✅ Import Customer
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Offers // ✅ Changed to PLURAL
{
    public class Offer : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ int for SQL

        public int CustomerId { get; set; } // ✅ int FK
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } // Navigation Property

        public string OfferTitle { get; set; }
        public string Status { get; set; } = "Pending";
        public string Description { get; set; }

        // ✅ Money should always be DECIMAL
        public decimal TotalAmount { get; set; }

        // ✅ SQL Requirement: Items must be their own table
        public virtual ICollection<OfferItem> OfferItems { get; set; }

        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? UpdatedAt { get; set; }
    }
}