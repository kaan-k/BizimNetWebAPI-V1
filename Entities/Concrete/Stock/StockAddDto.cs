using Core.Entities.Abstract;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Stock
{
    public class StockAddDto:IDto
    {
        public string Name { get; set; }
        //public string WarehouseId { get; set; }
        public int Count { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
