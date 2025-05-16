using Business.Abstract;
using Core.Enums;
using Entities.Concrete.Offer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }
        [HttpPost("Add")]
        public IActionResult Add(OfferDto offer)
        {
            var result = _offerService.Add(offer);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("Update")]
        public IActionResult Update(Offer offer)
        {
            var result = _offerService.Update(offer);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("Approve")]
        public IActionResult Approve(string id)
        {
            var result = _offerService.Approve(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("Reject")]
        public IActionResult Reject(string id, string reason)
        {
            var result = _offerService.Reject(id, reason);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _offerService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _offerService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _offerService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetByCustomerId")]
        public IActionResult GetByCustomerId(string customerId)
        {
            var result = _offerService.GetByCustomerId(customerId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetByEmployeeId")]
        public IActionResult GetByEmployeeId(string employeeId)
        {
            var result = _offerService.GetByEmployeeId(employeeId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("GetByStatus")]
        public IActionResult GetByStatus(OfferStatus status)
        {
            var result = _offerService.GetByStatus(status);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
        [HttpGet("CountByStatus")]
        public IActionResult GetOfferCountByStatus(OfferStatus status)
        {
            var result = _offerService.GetOfferCountByStatus(status);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}
