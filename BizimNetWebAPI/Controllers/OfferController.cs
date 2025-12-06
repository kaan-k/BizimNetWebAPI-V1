using Business.Abstract;
using Entities.Concrete.Offers;
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

        [HttpGet("GenerateOfferReport")]
        public IActionResult GenerateOfferReport(int offerId)
        {
            var result = _offerService.GenerateOfferReport(offerId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }


        [HttpGet("Approve")]
        public IActionResult Approve(int id)
        {
            var result = _offerService.Approve(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _offerService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetByStatus")]
        public IActionResult GetByStatus(string status)
        {
            var result = _offerService.GetByStatus(status);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _offerService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _offerService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }


        [HttpGet("GetByCustomerId")]
        public IActionResult GetByCustomerId(int customerId)
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
