using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete.Orders
{
    public class Order : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string OrderNumber { get; set; } // UUID
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; } = false;
        public decimal TotalAmount { get; set; }

        public int TableId { get; set; } // Which table?

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}