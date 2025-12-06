using Core.Entities.Abstract;

namespace Entities.Concrete.Offers
{
    public class OfferItemDto : IDto
    {
        public int Id { get; set; }
        public int OfferId { get; set; }

        public int StockId { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
