using Core.Entities.Abstract;
using System;

namespace Entities.Concrete.InstallationRequests // ✅ Changed to PLURAL
{
    public class InstallationRequestDto : IDto
    {
        public int Id { get; set; } // Added Id (useful for updates)

        public int OfferId { get; set; } // ✅ Changed string -> int

        public int CustomerId { get; set; } // ✅ Changed string -> int

        public int? AssignedEmployeeId { get; set; } // ✅ Changed string? -> int?

        public DateTime? CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public bool IsAssigned { get; set; }
        public bool IsCompleted { get; set; }

        public string? InstallationNote { get; set; }
    }
}