using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Duty
{
    public class DutyDto:IDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; }
    }
}
