using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFileDetailsDto:IDto
    {
        public string Id { get; set; }
        public string? OfferId { get; set; }

        public string DepartmentId { get; set; } 
        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModifiedAt { get; set; }

    }
}
