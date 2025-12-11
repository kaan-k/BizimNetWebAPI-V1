using Core.Entities.Abstract;
using Entities.Concrete.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Orders
{
    public class OrderSaveDto : IDto
    {
        public int TableId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
