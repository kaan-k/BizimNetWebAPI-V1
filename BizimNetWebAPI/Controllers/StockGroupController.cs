using Business.Abstract;
using Entities.Concrete.Stocks;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockGroupController : ControllerBase
    {
        private readonly IStockGroupService _stockGroupService;

        public StockGroupController(IStockGroupService stockGroupService)
        {
            _stockGroupService = stockGroupService;
        }

        [HttpPost("Add")]
        public IActionResult Add(StockGroupAddDto dto)
        {
            var result = _stockGroupService.Add(dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(StockGroup stockGroup)
        {
            var result = _stockGroupService.Update(stockGroup);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id) // Using POST for Delete based on your previous patterns
        {
            var result = _stockGroupService.Delete(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _stockGroupService.GetAll();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _stockGroupService.GetById(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}