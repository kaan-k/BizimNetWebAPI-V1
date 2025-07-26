using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.DocumentFile
{
    public class DocumentFileAddRequest
    {
        public IFormFile File { get; set; }

        public string? OfferId { get; set; }

        public string DepartmentId { get; set; } 
        public string DocumentName { get; set; }  
    }

}
