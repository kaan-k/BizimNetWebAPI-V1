using Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete.Offers
{
    public class OfferItem : IEntity
    {
        [Key]
        public int Id { get; set; }

        // ✅ Link to the Offer Table
        public int OfferId { get; set; }
        [ForeignKey("OfferId")]
        public virtual Offer Offer { get; set; }

        public int StockId { get; set; } // Assuming you will fix Stock entity later
        public string StockName { get; set; } // Snapshot of name at time of offer

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // ✅ Decimal
        public decimal TotalPrice { get; set; } // ✅ Decimal
    }
}