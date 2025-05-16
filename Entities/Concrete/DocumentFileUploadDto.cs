using Core.Entities.Abstract;
using System.Collections.Generic;

namespace Entities.Concrete.DocumentFiles
{
    public class DocumentFileUploadDto : IDto
    {
        public string PersonId { get; set; }
        public string DocumentName { get; set; }
        public List<string> downloderIds { get; set; } = new List<string>();
        public string? DocumentPath { get; set; }
        public string DepartmentId { get; set; }
        public string? DocumentFullName { get; set; }

    }
}
