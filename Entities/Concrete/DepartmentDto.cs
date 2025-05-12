using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class DepartmentDto
    {
        public string Name { get; set; }

        public string ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
