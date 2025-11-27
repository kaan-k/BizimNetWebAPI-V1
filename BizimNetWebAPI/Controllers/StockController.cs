using Business.Abstract;
using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Stock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("Add")]
        public IActionResult Add(StockAddDto stock)
        {
            var result = _stockService.Add(stock);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Stock stock)
        {
            var result = _stockService.Update(stock);
            return Ok(result);
        }

        [HttpGet("GetByDeviceType")]
        public IActionResult GetByDeviceType(DeviceType deviceType)
        {
            var result = _stockService.GetByDeviceType(deviceType);
            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _stockService.Delete(id);
            return Ok(result);
        }
    }
}