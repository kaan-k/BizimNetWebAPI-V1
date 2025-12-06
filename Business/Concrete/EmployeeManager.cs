using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Departments; // ✅ Plural Namespace
using Entities.Concrete.Employees; // ✅ Plural Namespace
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;
        private readonly IMapper _mapper;
        private readonly IDepartmentDal _departmentDal;

        public EmployeeManager(IEmployeeDal employeeDal, IDepartmentDal departmentDal, IMapper mapper)
        {
            _employeeDal = employeeDal;
            _departmentDal = departmentDal;
            _mapper = mapper;
        }

        public IResult Add(EmployeeDto employeeDto)
        {
            var employeeMap = _mapper.Map<Employee>(employeeDto);
            // SQL generates ID automatically
            _employeeDal.Add(employeeMap);
            return new SuccessResult("Eklendi");
        }

        public IResult AddRange(List<EmployeeDto> employees)
        {
            if (employees == null || !employees.Any())
                return new ErrorResult("Gönderilen çalışan listesi boş.");

            foreach (var employeeDto in employees)
            {
                var employee = _mapper.Map<Employee>(employeeDto);
                _employeeDal.Add(employee);
            }
            return new SuccessResult("Çalışanlar başarıyla eklendi.");
        }

        public IResult AssignRole(int employeeId, string role)
        {
            var emp = _employeeDal.Get(x => x.Id == employeeId);
            if (emp == null)
            {
                return new ErrorResult("Çalışan bulunamadı.");
            }
            if (emp.Role == role)
            {
                return new ErrorResult("Çalışan zaten bu rolde.");
            }
            emp.Role = role;
            emp.LastUpdated = DateTime.UtcNow; // ✅ Use UtcNow

            _employeeDal.Update(emp);
            return new SuccessResult("Çalışan rolü güncellendi.");
        }

        public IResult AssignToDepartment(int employeeId, int departmentId)
        {
            var emp = _employeeDal.Get(x => x.Id == employeeId);
            if (emp == null)
            {
                return new ErrorResult("Çalışan bulunamadı.");
            }

            // ✅ Fixed Typo: DepartmentId
            emp.DepartmentId = departmentId;
            emp.LastUpdated = DateTime.UtcNow;

            _employeeDal.Update(emp);
            return new SuccessResult("Çalışan departmanı güncellendi.");
        }

        public IResult Delete(int id)
        {
            _employeeDal.Delete(id);
            return new SuccessResult("Silindi.");
        }

        public IDataResult<List<Employee>> GetAll()
        {
            var allEmps = _employeeDal.GetAll();
            return new SuccessDataResult<List<Employee>>(allEmps);
        }

        public IDataResult<List<Employee>> GetByDepartmentId(int departmentId)
        {
            // ✅ Fixed Typo: DepartmentId
            var emps = _employeeDal.GetAll(x => x.DepartmentId == departmentId);
            return new SuccessDataResult<List<Employee>>(emps);
        }

        public IDataResult<Employee> GetById(int id)
        {
            var emp = _employeeDal.Get(x => x.Id == id);
            if (emp == null)
            {
                return new ErrorDataResult<Employee>(emp, "Çalışan bulunamadı.");
            }
            return new SuccessDataResult<Employee>(emp);
        }

        public IDataResult<List<Employee>> GetByRole(string role)
        {
            var emp = _employeeDal.GetAll(x => x.Role == role);
            if (emp == null) // This check is technically redundant as GetAll returns empty list, not null
            {
                return new ErrorDataResult<List<Employee>>(emp, "Çalışan bulunamadı.");
            }
            return new SuccessDataResult<List<Employee>>(emp);
        }

        public IDataResult<int> GetCountByDepartment(int departmentId)
        {
            // ✅ Fixed Typo: DepartmentId
            var count = _employeeDal.GetAll(x => x.DepartmentId == departmentId).Count;
            return new SuccessDataResult<int>(count);
        }

        public IDataResult<int> GetCountByRole(string role)
        {
            var count = _employeeDal.GetAll(x => x.Role == role).Count;
            return new SuccessDataResult<int>(count);
        }

        public IDataResult<Employee> GetManagerByDepartment(int departmentId)
        {
            var department = _departmentDal.Get(x => x.Id == departmentId);
            if (department == null) return new ErrorDataResult<Employee>("Departman bulunamadı");

            // ManagerId is int now
            var manager = _employeeDal.Get(x => x.Id == department.ManagerId);
            return new SuccessDataResult<Employee>(manager);
        }

        public IDataResult<int> GetTotalEmployeeCount()
        {
            var count = _employeeDal.GetAll().Count;
            return new SuccessDataResult<int>(count);
        }

        public IResult ReturnEmployeeEmail(int employeeId)
        {
            var employee = _employeeDal.Get(X => X.Id == employeeId);
            if (employee == null) return new ErrorResult("Çalışan bulunamadı");

            return new SuccessResult(employee.Email);
        }

        public IResult Update(EmployeeDto employee, int employeeId)
        {
            var existingEmployee = _employeeDal.Get(x => x.Id == employeeId);
            if (existingEmployee == null)
            {
                return new ErrorResult("Güncellenecek çalışan bulunamadı.");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Surname = employee.Surname;
            existingEmployee.Role = employee.Role;
            existingEmployee.DepartmentId = employee.DeparmentId; // ✅ Fixed Typo in Entity
            existingEmployee.LastUpdated = DateTime.UtcNow;

            _employeeDal.Update(existingEmployee);
            return new SuccessResult("Çalışan başarıyla güncellendi.");
        }

        public IResult UpdateRange(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
            {
                return new ErrorResult("Gönderilen çalışan listesi boş.");
            }

            var notFoundIds = new List<int>();

            foreach (var employee in employees)
            {
                var existingEmployee = _employeeDal.Get(x => x.Id == employee.Id);
                if (existingEmployee == null)
                {
                    notFoundIds.Add(employee.Id);
                    continue;
                }

                existingEmployee.Name = employee.Name;
                existingEmployee.Surname = employee.Surname;
                existingEmployee.Role = employee.Role;
                existingEmployee.DepartmentId = employee.DepartmentId; // ✅ Fixed Typo
                existingEmployee.LastUpdated = DateTime.UtcNow;

                _employeeDal.Update(existingEmployee);
            }

            if (notFoundIds.Any())
            {
                return new ErrorResult($"Bazı çalışanlar bulunamadı ve güncellenemedi. Eksik ID'ler: {string.Join(", ", notFoundIds)}");
            }

            return new SuccessResult("Tüm çalışanlar başarıyla güncellendi.");
        }
    }
}