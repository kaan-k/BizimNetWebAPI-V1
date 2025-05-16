using Business.Abstract;
using Entities.Concrete.InstallationRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallationRequestController : ControllerBase
    {
        private readonly IInstallationRequestService _installationRequestService;

        public InstallationRequestController(IInstallationRequestService installationRequestService)
        {
            _installationRequestService = installationRequestService;
        }
        [HttpPost("Add")]
        public IActionResult Add(InstallationRequestDto request)
        {
            var result = _installationRequestService.Add(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("Update")]
        public IActionResult Update(InstallationRequest request)
        {
            var result = _installationRequestService.Update(request);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("UpdateNote")]
        public IActionResult UpdateNote(string requestId, string note)
        {
            var result = _installationRequestService.UpdateNote(requestId, note);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("AssignEmployee")]
        public IActionResult AssignEmployee(string requestId, string employeeId)
        {
            var result = _installationRequestService.AssignEmployee(requestId, employeeId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("MarkAsCompleted")]
        public IActionResult MarkAsCompleted(string requestId)
        {
            var result = _installationRequestService.MarkAsCompleted(requestId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _installationRequestService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetByOfferId")]
        public IActionResult GetByOfferId(string offerId)
        {
            var result = _installationRequestService.GetByOfferId(offerId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetAssigned")]
        public IActionResult GetAssigned()
        {
            var result = _installationRequestService.GetAssigned();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetUnassigned")]
        public IActionResult GetUnassigned()
        {
            var result = _installationRequestService.GetUnassigned();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _installationRequestService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _installationRequestService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetByCustomerId")]
        public IActionResult GetByCustomerId(string customerId)
        {
            var result = _installationRequestService.GetByCustomerId(customerId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

    }
}
