using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            return Ok(result);
        }

        [HttpPost("RecievePay")]
        public IActionResult RecievePay(string billId,int amount)
        {
            var result = _billingService.RecievePay(billId,amount);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(Billing billing, string id)
        {
            var result = _billingService.Update(billing, id);
            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _billingService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _billingService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _billingService.GetAll();
            return Ok(result);
        }
    }
}