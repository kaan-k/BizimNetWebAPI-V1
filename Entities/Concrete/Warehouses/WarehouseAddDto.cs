using Core.Entities.Abstract;
using Entities.Concrete.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Warehouses
{
    public class WarehouseAddDto:IDto
    {


        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsMainWarehouse { get; set; }

        public ICollection<Stock>? Stocks { get; set; }
    }
}
