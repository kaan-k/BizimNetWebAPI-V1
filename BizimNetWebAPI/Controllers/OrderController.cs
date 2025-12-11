using Business.Abstract;
using Entities.Concrete.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // --- POS SPECIFIC ENDPOINTS ---

        [HttpGet("GetActiveOrder")]
        public IActionResult GetActiveOrder(int tableId)
        {
            var result = _orderService.GetActiveOrderByTableId(tableId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("SaveOrder")]
        public IActionResult SaveOrder(OrderSaveDto saveDto)
        {
            var result = _orderService.SaveOrder(saveDto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("PayOrder")]
        public IActionResult PayOrder(int id)
        {
            var result = _orderService.PayOrder(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // --- STANDARD CRUD ---

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _orderService.GetAll();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _orderService.GetById(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Add")]
        public IActionResult Add(Order order)
        {
            var result = _orderService.Add(order);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Order order)
        {
            var result = _orderService.Update(order);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _orderService.Delete(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}