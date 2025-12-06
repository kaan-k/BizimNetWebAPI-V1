using Core.Entities.Abstract;
using Core.Enums;

namespace Entities.Concrete.Stocks // ✅ Changed to PLURAL
{
    public class StockAddDto : IDto
    {
        public string Name { get; set; }

        // public int WarehouseId { get; set; } // ✅ Changed string -> int if you use it later

        public int Count { get; set; }

        public DeviceType DeviceType { get; set; }
    }
}