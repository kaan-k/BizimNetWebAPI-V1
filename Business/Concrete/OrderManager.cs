using Business.Abstract;
using Business.Constants; // Assuming you have messages here
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Orders;
using Entities.Concrete.Tables;
using Core.Enums; // For TableStatus
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderDetailDal _orderDetailDal;
        private readonly ITableDal _tableDal;

        public OrderManager(IOrderDal orderDal, IOrderDetailDal orderDetailDal, ITableDal tableDal)
        {
            _orderDal = orderDal;
            _orderDetailDal = orderDetailDal;
            _tableDal = tableDal;
        }

        public IDataResult<OrderDto> GetActiveOrderByTableId(int tableId)
        {
            // 1. Find the open order for this table
            var order = _orderDal.Get(o => o.TableId == tableId && !o.IsPaid);

            // ✅ CRITICAL CHECK: If no order exists, stop here!
            if (order == null)
            {
                // Return an empty/null DTO so the frontend knows the table is clean
                return new DataResult<OrderDto>(null, true);
            }

            // 2. Fetch lines ONLY if order is NOT null
            var details = _orderDetailDal.GetAll(d => d.OrderId == order.Id);

            // 3. Map to DTO
            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Items = details.Select(d => new OrderItemDto
                {
                    StockId = d.StockId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            return new SuccessDataResult<OrderDto>(orderDto, "Sipariş getirildi.");
        }

        public IResult SaveOrder(OrderSaveDto saveDto)
        {
            // 1. Check if Order exists
            var existingOrder = _orderDal.Get(o => o.TableId == saveDto.TableId && !o.IsPaid);

            if (existingOrder == null)
            {
                // --- CREATE NEW ORDER ---
                existingOrder = new Order
                {
                    TableId = saveDto.TableId,
                    OrderNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                    CreatedAt = DateTime.UtcNow,
                    IsPaid = false,
                    TotalAmount = 0
                };
                _orderDal.Add(existingOrder);

                // --- UPDATE TABLE STATUS TO BUSY ---
                var table = _tableDal.Get(t => t.Id == saveDto.TableId);
                if (table != null)
                {
                    table.CurrentStatus = TableStatus.Busy;
                    _tableDal.Update(table);
                }
            }
            else
            {
                // --- UPDATE EXISTING ---
                // Strategy: Wipe old details and re-insert (Cleanest for full-cart saves)
                var oldDetails = _orderDetailDal.GetAll(d => d.OrderId == existingOrder.Id);
                foreach (var detail in oldDetails)
                {
                    _orderDetailDal.Delete(detail);
                }
            }

            // 2. Insert New Details
            decimal calculatedTotal = 0;

            foreach (var item in saveDto.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = existingOrder.Id,
                    StockId = item.StockId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                _orderDetailDal.Add(orderDetail);

                calculatedTotal += (item.Quantity * item.UnitPrice);
            }

            // 3. Update Header Total
            existingOrder.TotalAmount = calculatedTotal;
            _orderDal.Update(existingOrder);

            return new SuccessResult("Sipariş başarıyla kaydedildi.");
        }

        public IResult PayOrder(int orderId)
        {
            var order = _orderDal.Get(o => o.Id == orderId);
            if (order == null) return new ErrorResult("Sipariş bulunamadı.");

            // Close Order
            order.IsPaid = true;
            _orderDal.Update(order);

            // Free the Table
            var table = _tableDal.Get(t => t.Id == order.TableId);
            if (table != null)
            {
                table.CurrentStatus = TableStatus.Free;
                _tableDal.Update(table);
            }

            return new SuccessResult("Ödeme alındı, masa kapatıldı.");
        }

        // --- Standard CRUD ---
        public IResult Add(Order order)
        {
            _orderDal.Add(order);
            return new SuccessResult("Sipariş eklendi.");
        }

        public IResult Delete(int id)
        {
            var order = _orderDal.Get(o => o.Id == id);
            if (order == null) return new ErrorResult("Kayıt bulunamadı");
            _orderDal.Delete(order);
            return new SuccessResult("Sipariş silindi.");
        }

        public IDataResult<List<Order>> GetAll()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll());
        }

        public IDataResult<Order> GetById(int id)
        {
            return new SuccessDataResult<Order>(_orderDal.Get(o => o.Id == id));
        }

        public IResult Update(Order order)
        {
            _orderDal.Update(order);
            return new SuccessResult("Sipariş güncellendi.");
        }
    }
}