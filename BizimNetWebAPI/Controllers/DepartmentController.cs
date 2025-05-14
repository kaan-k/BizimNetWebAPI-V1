using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizimNetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("Add")]
        public IActionResult Add(DepartmentDto department)
        {
            var result = _departmentService.Add(department);
            return Ok(result);
        }
        [HttpPost("Update")]
        public IActionResult Update(Department department, string departmentId)
        {
            var result = _departmentService.Update(department, departmentId);
            return Ok(result);
        }
        [HttpPost("AssignManager")]
        public IActionResult AssignManager(string departmentId, string employeeId)
        {
            var result = _departmentService.AssignManager(departmentId, employeeId);
            return Ok(result);
        }
        [HttpGet("Delete")]
        public IActionResult Delete(string id)
        {
            var result = _departmentService.Delete(id);
            return Ok(result);
        }
        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var result = _departmentService.GetById(id);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _departmentService.GetAll();
            return Ok(result);
        }
        [HttpGet("SearchByName")]
        public IActionResult SearchByName(string name)
        {
            var result = _departmentService.SearchByName(name);
            return Ok(result);
        }
        [HttpGet("GetEmployeesByDepartmentId")]
        public IActionResult GetEmployeesByDepartmentId(string departmentId)
        {
            var result = _departmentService.GetEmployeesByDepartmentId(departmentId);
            return Ok(result);
        }
        [HttpGet("GetEmployeeCount")]
        public IActionResult GetEmployeeCount(string departmentId)
        {
            var result = _departmentService.GetEmployeeCount(departmentId);
            return Ok(result);
        }
        [HttpGet("IsDepartmentNameTaken")]
        public IActionResult IsDepartmentNameTaken(string name)
        {
            var result = _departmentService.IsDepartmentNameTaken(name);
            return Ok(result);
        }
        [HttpGet("GetManagerOfDepartment")]
        public IActionResult GetManagerOfDepartment(string departmentId)
        {
            var result = _departmentService.GetManagerOfDepartment(departmentId);
            return Ok(result);
        }
        [HttpGet("GetEmployeesByRole")]
        public IActionResult GetEmployeesByRole(string departmentId, string role)
        {
            var result = _departmentService.GetEmployeesByRole(departmentId, role);
            return Ok(result);
        }

    }
}
