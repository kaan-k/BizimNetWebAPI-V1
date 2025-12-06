using Core.Entities.Abstract;
using System;

namespace Entities.Concrete.Duties // ✅ Changed to Plural
{
    public class DutyDetailsDto : IDto
    {
        // Note: DTOs usually need an Id to identify the record being updated/viewed
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        // ✅ Changed string -> int
        public int CustomerId { get; set; }

        public string Priority { get; set; }
        public DateTime? Deadline { get; set; }

        public DateTime? BeginsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? Status { get; set; }

        // ✅ Changed string -> int? (Nullable int)
        // These are nullable because a duty might not be completed yet.
        public int? CreatedBy { get; set; }
        public int? CompletedBy { get; set; }
        public int? AssignedEmployeeId { get; set; }

        public bool CompletedBeforeDeadline { get; set; }
        public string? SignatureBase64 { get; set; }
    }
}