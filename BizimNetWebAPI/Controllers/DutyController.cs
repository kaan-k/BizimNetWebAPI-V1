using Business.Abstract;
using Entities.Concrete.Duties;
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

        [HttpPost("AddCompleted")]
        public IActionResult AddCompleted([FromBody] DutyDto request)
        {
            var result = _dutyService.AddCompleted(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _dutyService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] DutyDto request)
        {
            var result = _dutyService.Update(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("MarkAsCompleted")]
        public IActionResult MarkAsCompleted(int id)
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
        public IActionResult GetAllDetails([FromQuery] int userId)
        {
            var result = _dutyService.GetAllDetails(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllByCustomer/{customerId}")]
        public IActionResult GetAllByCustomerId(int customerId)
        {
            var result = _dutyService.GetAllByCustomerIdReport(customerId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetAllByEmployee/{employeeId}")]
        public IActionResult GetAllByEmployeeId(int employeeId)
        {
            var result = _dutyService.GetAllByEmployeeId(employeeId);
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
        public IActionResult UpdateStatusById([FromQuery] int id, [FromQuery] string newStatus)
        {
            var result = _dutyService.UpdateStatusById(id, newStatus);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _dutyService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("ReplaceById")]
        public IActionResult ReplaceById(int id, int toReplaceId)
        {
            var result = _dutyService.ReplaceCustomerId(id, toReplaceId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
