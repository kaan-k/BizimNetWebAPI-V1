using Core.Utilities.Results;
using Entities.Concrete.Orders;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IOrderService
    {
        // POS Specific Methods
        IDataResult<OrderDto> GetActiveOrderByTableId(int tableId);
        IResult SaveOrder(OrderSaveDto saveDto);
        IResult PayOrder(int orderId); // To close the table

        // Standard CRUD
        IDataResult<Order> GetById(int id);
        IDataResult<List<Order>> GetAll();
        IResult Add(Order order);
        IResult Delete(int id);
        IResult Update(Order order);
    }
}