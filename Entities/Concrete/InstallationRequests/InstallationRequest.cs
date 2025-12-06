using Core.Entities.Abstract;
using Entities.Concrete.Customers; // ✅ Import Customer namespace
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.InstallationRequests // ✅ Changed to PLURAL (InstallationRequest -> InstallationRequests)
{
    public class InstallationRequest : IEntity
    {
        [Key]
        public int Id { get; set; } // ✅ Changed from string to int

        public int OfferId { get; set; } // ✅ Changed from string to int

        // ✅ Customer Relationship
        public int CustomerId { get; set; } // Foreign Key
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } // Navigation Property

        public int? AssignedEmployeeId { get; set; } // ✅ Changed from string? to int?

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? FinishedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public bool IsAssigned { get; set; } = false;
        public bool IsCompleted { get; set; }

        public string? InstallationNote { get; set; }
    }
}