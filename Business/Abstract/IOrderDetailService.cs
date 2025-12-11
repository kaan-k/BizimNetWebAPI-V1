using Core.Utilities.Results;
using Entities.Concrete.Orders;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IOrderDetailService
    {
        IResult Add(OrderDetail orderDetail);
        IResult Update(OrderDetail orderDetail);
        IResult Delete(int id);
        IDataResult<OrderDetail> GetById(int id);
        IDataResult<List<OrderDetail>> GetAll();

        // Helper: Get items for a specific order ID
        IDataResult<List<OrderDetail>> GetByOrderId(int orderId);
    }
}