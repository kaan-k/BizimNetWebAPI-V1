using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFileUpdateRequest
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
        public string DepartmentId { get; set; }
        public string? OfferId { get; set; }

        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentFullName { get; set; }
        public string? DocumentType { get; set; }

    }

}
