using Core.Entities.Abstract;
using Entities.Concrete.Customers; // ✅ Import the Customer namespace
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Duties
{
    public class Duty : IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; } // Foreign Key
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public string Priority { get; set; }

        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }
        public DateTime? BeginsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? Status { get; set; }

        // ✅ User/Employee IDs
        // I changed these to int?. When you fix your User/Employee class,
        // you can come back and add navigation properties (e.g., virtual User CreatedByUser).
        public int? CreatedBy { get; set; }
        public int? CompletedBy { get; set; }
        public int? AssignedEmployeeId { get; set; }

        public bool CompletedBeforeDeadline { get; set; }

        public string? SignatureBase64 { get; set; }
    }
}