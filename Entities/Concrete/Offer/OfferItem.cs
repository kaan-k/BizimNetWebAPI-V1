using Core.Entities.Abstract;
using Entities.Concrete.Stocks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Concrete.Offers
{
    public class OfferItem : IEntity
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int OfferId { get; set; }
        public string StockName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
