using Business.Abstract;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessUserController : ControllerBase
    {
        private readonly IBusinessUserService _businessUserService;

        public BusinessUserController(IBusinessUserService businessUserService)
        {
            _businessUserService = businessUserService;
        }

        
        [HttpPost("Add")]
        public IActionResult Add(BusinessUserDto businessUser)
        {
            var register = _businessUserService.Add(businessUser);
            if (!register.Success)
            {
                return BadRequest(register.Message);
            }

            var check = _businessUserService.CreateAccessToken(register.Data);
            if (!check.Success)
            {
                return BadRequest(check.Message);
            }
            return Ok(check);
        }

        [HttpPost("Login")]
        public IActionResult Login(BusinessUserLoginDto userForLoginDto)
        {
            var result = _businessUserService.UserLogin(userForLoginDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var res = _businessUserService.CreateAccessToken(result.Data);
            if (res.Success)
            {
                // HttpContext.Response.Headers.Add("Authorization", "Bearer " + res.Data.Token); // Optional, usually returned in body
                return Ok(res);
            }
            return BadRequest(res.Message);
        }

        [Authorize]
        [HttpPost("Update")]
        public IActionResult Update(BusinessUser businessUser, int id) // ✅ Changed string -> int
        {
            var result = _businessUserService.Update(businessUser, id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(BusinessUserPasswordResetDto businessUser)
        {
            var result = _businessUserService.ResetPassword(businessUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id) // ✅ Changed string -> int
        {
            var result = _businessUserService.Delete(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpGet("GetById")]
        public IActionResult Get(int id) // ✅ Changed string -> int
        {
            var result = _businessUserService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _businessUserService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}