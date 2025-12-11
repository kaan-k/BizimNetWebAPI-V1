using Core.Entities.Abstract;
using Entities.Concrete.Stocks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Orders
{
    public class OrderDetail : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        public int StockId { get; set; }
        [ForeignKey(nameof(StockId))]
        public virtual Stock Stock { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Snapshot price
    }
}