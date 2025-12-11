using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Tables
{
    public class TableDetailDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SectionId { get; set; }
        public TableStatus CurrentStatus { get; set; }
        public int SortOrder { get; set; }
        public decimal CurrentBillAmount { get; set; }
    }
}
