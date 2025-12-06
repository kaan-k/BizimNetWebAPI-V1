using Business.Abstract;
using Entities.Concrete.Payments; // ✅ Correct Namespace
using Entities.DTOs.BillingDtos;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("Add")]
        public IActionResult Add(BillingDto billingDto)
        {
            var result = _billingService.Add(billingDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // ✅ Fixed Typo (Recieve -> Receive)
        // ✅ Fixed Types (string -> int for ID, int -> decimal for Amount)
        //[HttpPost("ReceivePay")]
        //public IActionResult ReceivePay(int billId, decimal amount)
        //{
        //    var result = _billingService.ReceivePay(billId, amount);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}

        [HttpPost("Update")]
        public IActionResult Update(Billing billing)
        {
            // Usually, ID is included in the Billing object now
            var result = _billingService.Update(billing);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id) // ✅ Changed string -> int
        {
            var result = _billingService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) // ✅ Changed string -> int
        {
            var result = _billingService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _billingService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}