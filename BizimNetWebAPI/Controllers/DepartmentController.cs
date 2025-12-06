using Business.Abstract;
using Entities.Concrete.Departments; // ✅ Plural Namespace
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
        public IActionResult Update(DepartmentDto department) // Updated to Dto based on Service
        {
            var result = _departmentService.Update(department);
            return Ok(result);
        }

        [HttpPost("AssignManager")]
        public IActionResult AssignManager(int departmentId, int employeeId) // ✅ Changed string -> int
        {
            var result = _departmentService.AssignManager(departmentId, employeeId);
            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(int id) // ✅ Changed string -> int
        {
            var result = _departmentService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) // ✅ Changed string -> int
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
        public IActionResult GetEmployeesByDepartmentId(int departmentId) // ✅ Changed string -> int
        {
            var result = _departmentService.GetEmployeesByDepartmentId(departmentId);
            return Ok(result);
        }

        [HttpGet("GetEmployeeCount")]
        public IActionResult GetEmployeeCount(int departmentId) // ✅ Changed string -> int
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
        public IActionResult GetManagerOfDepartment(int departmentId) // ✅ Changed string -> int
        {
            var result = _departmentService.GetManagerOfDepartment(departmentId);
            return Ok(result);
        }

        [HttpGet("GetEmployeesByRole")]
        public IActionResult GetEmployeesByRole(int departmentId, string role) // ✅ Changed string -> int
        {
            var result = _departmentService.GetEmployeesByRole(departmentId, role);
            return Ok(result);
        }
    }
}