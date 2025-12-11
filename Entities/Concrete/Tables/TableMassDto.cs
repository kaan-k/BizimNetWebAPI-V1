using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Tables
{
    public class TableMassAddDto : IDto
    {
        public int SectionId { get; set; }
        public string Prefix { get; set; } // e.g., "S-"
        public int StartNumber { get; set; } = 1; // Start from 1, or 11 if adding more later
        public int Count { get; set; } // How many to add
    }
}
