using Core.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace Entities.Concrete.Services // ✅ Changed to PLURAL
{
    public class ServicingAddDto : IDto
    {
        public string Name { get; set; }

        // Optional: You can keep TrackingId here if you want to allow manual override, 
        // otherwise let the Entity handle the generation.
        // public string TrackingId { get; set; } 

        public List<int> DeviceIds { get; set; } // ✅ Changed string -> int

        public int CustomerId { get; set; } // ✅ Changed string -> int

        public string Status { get; set; }

        public DateTime? LastActionDate { get; set; }
        public string? LastAction { get; set; }

        // Usually, CreatedAt is set by the server, not the DTO, but leaving it here is fine.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}