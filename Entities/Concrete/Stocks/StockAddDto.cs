using Core.Entities.Abstract;
using Core.Enums;

public class StockAddDto : IDto
{
    public string Name { get; set; }

    // --- NEW POS FIELDS ---
    public decimal UnitPrice { get; set; } // Critical for Sales
    public string? Barcode { get; set; }   // For Scanner
    public string? ImageUrl { get; set; }  // For Grid UI
    public int StockGroupId { get; set; }  // The Category ID (Foreign Key)

    // --- EXISTING FIELDS ---
    public int Count { get; set; }
    public DeviceType DeviceType { get; set; }
    public int WarehouseId { get; set; }

    public bool IsActive { get; set; }
}
