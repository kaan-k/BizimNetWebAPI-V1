using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Tables
{
    public class TableAddDto:IDto
    {
        public string Name { get; set; }
        public int SectionId { get; set; }
        public int SortOrder { get; set; } // Important for the grid!
    }
}
