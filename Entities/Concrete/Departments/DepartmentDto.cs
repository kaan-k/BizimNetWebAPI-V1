using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Departments
{
    public class DepartmentDto:IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // ✅ Changed from string to int
        // Note: Once you fix your "Person" or "User" class, 
        // you should come back and add a Navigation Property here.
        public int ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // ✅ Use UtcNow
        public DateTime? UpdatedAt { get; set; }
    }
}
