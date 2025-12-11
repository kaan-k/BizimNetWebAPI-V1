using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Orders;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public IResult Add(OrderDetail orderDetail)
        {
            _orderDetailDal.Add(orderDetail);
            return new SuccessResult("Sipariş detayı eklendi.");
        }

        public IResult Delete(int id)
        {
            var detail = _orderDetailDal.Get(d => d.Id == id);
            if (detail == null)
            {
                return new ErrorResult("Detay bulunamadı.");
            }
            _orderDetailDal.Delete(detail);
            return new SuccessResult("Sipariş detayı silindi.");
        }

        public IDataResult<List<OrderDetail>> GetAll()
        {
            return new SuccessDataResult<List<OrderDetail>>(_orderDetailDal.GetAll());
        }

        public IDataResult<OrderDetail> GetById(int id)
        {
            return new SuccessDataResult<OrderDetail>(_orderDetailDal.Get(d => d.Id == id));
        }

        public IDataResult<List<OrderDetail>> GetByOrderId(int orderId)
        {
            return new SuccessDataResult<List<OrderDetail>>(_orderDetailDal.GetAll(d => d.OrderId == orderId));
        }

        public IResult Update(OrderDetail orderDetail)
        {
            _orderDetailDal.Update(orderDetail);
            return new SuccessResult("Sipariş detayı güncellendi.");
        }
    }
}