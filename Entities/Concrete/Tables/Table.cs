using Core.Entities.Abstract;
using Core.Enums;
using Entities.Concrete.Sections;
using Entities.Concrete.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Tables
{
    public class Table:IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }


        public TableStatus CurrentStatus { get; set; } = TableStatus.Free;
        public int SortOrder { get; set; }


    }
}
