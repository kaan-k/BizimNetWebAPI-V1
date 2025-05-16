using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete.DocumentFiles
{
    public class DocumentFileDetailsDto:IDto
    {
        public string Id { get; set; }
        public string PersonName { get; set; }
        public List<string> downloderIds { get; set; } = new List<string>();
        public string DocumentName { get; set; }
        public string? DocumentPath { get; set; }
        public string DepartmentId { get; set; }
        public string? DocumentFullName { get; set; }

    }
}
