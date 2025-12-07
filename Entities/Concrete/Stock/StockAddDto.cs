using Core.Entities.Abstract;
using Core.Enums;

public class StockAddDto : IDto
{
    public string Name { get; set; }
    public int Count { get; set; }
    public DeviceType DeviceType { get; set; }
    public int WarehouseId { get; set; }  // ✅ EKLENDİ
}
