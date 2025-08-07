using Business.Abstract;
using Entities.Concrete.Duty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DutyController : ControllerBase
    {
        private readonly IDutyService _dutyService;

        public DutyController(IDutyService dutyService)
        {
            _dutyService = dutyService;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] DutyDto request)
        {
            var result = _dutyService.Add(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _dutyService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] Duty request)
        {
            var result = _dutyService.Update(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("MarkAsCompleted")]
        public IActionResult MarkAsCompleted(string id)
        {
            var result = _dutyService.MarkAsCompleted(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _dutyService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetAllDetails")]
        public IActionResult GetAllDetails()
        {
            var result = _dutyService.GetAllDetails();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetAllByCustomer/{customerId}")]
        public IActionResult GetAllByCustomerId(string customerId)
        {
            var result = _dutyService.GetAllByCustomerId(customerId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllByStatus/{status}")]
        public IActionResult GetAllByStatus(string status)
        {
            var result = _dutyService.GetAllByStatus(status);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("UpdateStatusById")]
        public IActionResult UpdateStatusById([FromQuery] string id, [FromQuery] string newStatus)
        {
            var result = _dutyService.UpdateStatusById(id, newStatus);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _dutyService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
