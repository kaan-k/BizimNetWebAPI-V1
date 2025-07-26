using Core.Entities.Abstract;
using System.Collections.Generic;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFileUploadDto : IDto
    {
        public string DepartmentId { get; set; }
        public string? OfferId { get; set; } 
        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; }

    }
}
