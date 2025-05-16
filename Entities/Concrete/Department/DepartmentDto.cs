using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Department
{
    public class DepartmentDto:IDto
    {
        public string Name { get; set; }

        public string? ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
