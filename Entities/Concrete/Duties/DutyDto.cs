using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Duties
{
    public class DutyDto : IDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; } // UI için

        public string? Priority { get; set; }

        public DateTime? Deadline { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
        public DateTime? BeginsAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? Status { get; set; }

        public int? CreatedBy { get; set; }
        public int? CompletedBy { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public string? AssignedEmployeeName { get; set; } // UI için

        public bool CompletedBeforeDeadline { get; set; }

        public string? SignatureBase64 { get; set; }
    }

}
