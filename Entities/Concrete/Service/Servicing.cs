using Core.Entities.Abstract;
using Entities.Concrete.Customers; // ✅ Import Customer
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Services // ✅ Changed to PLURAL (Servicing -> Services)
{
    public class Servicing : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed to int

        public string Name { get; set; }

        // Logic kept same, just ensuring it generates a string
        public string TrackingId { get; set; } = $"ARY{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

        // ⚠️ SQL cannot store lists directly. 
        // Marked [NotMapped] to prevent errors. 
        // Ideally, create a 'ServiceDevice' join table later.
        [NotMapped]
        public List<int> DeviceIds { get; set; } = new List<int>();

        // ✅ Customer Relationship
        public int CustomerId { get; set; } // Foreign Key
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } // Navigation Property

        public string Status { get; set; }

        public DateTime? LastActionDate { get; set; }
        public string? LastAction { get; set; } // Made nullable to be safe

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? UpdatedAt { get; set; }
    }
}