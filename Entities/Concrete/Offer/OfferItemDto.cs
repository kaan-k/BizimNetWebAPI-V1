using Core.Entities.Abstract;

namespace Entities.Concrete.Offers
{
    public class OfferItemDto : IDto
    {
        public int Id { get; set; } // Added ID
        public int StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // ✅ Decimal
        public decimal TotalPrice { get; set; } // ✅ Decimal
    }
}