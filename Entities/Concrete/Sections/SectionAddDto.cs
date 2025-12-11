using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Sections
{
    public class SectionAddDto : IDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
