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
        [HttpGet("Approve")]
        public IActionResult Approve(string id)
        {
            var result = _offerService.Approve(id);
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


        [HttpGet("GetAllDetails")]
        public IActionResult GetAllDetails()
        {
            var result = _offerService.GetAllDetails();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}
