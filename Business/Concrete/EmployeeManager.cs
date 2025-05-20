using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;
        private readonly IDepartmentDal _departmentDal;
        public EmployeeManager(IEmployeeDal employeeDal, IDepartmentDal departmentDal) {
            _employeeDal = employeeDal;
            _departmentDal = departmentDal;
        }
        
        public IResult Add(EmployeeDto employee)
        {
            var employeeMap = new Employee
            {
                DeparmentId = employee.DeparmentId,
                Name = employee.Name,
                Surname = employee.Surname,
                Role = employee.Role
            };
            _employeeDal.Add(employeeMap);

            return new SuccessResult("Eklendi");
        }

        public IResult AddRange(List<EmployeeDto> employees)
        {
            if (employees == null || !employees.Any())
                return new ErrorResult("Gönderilen çalışan listesi boş.");
            foreach (var employeeDto in employees)
            {
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Surname = employeeDto.Surname,
                    DeparmentId = employeeDto.DeparmentId,
                    Role = employeeDto.Role
                };
                _employeeDal.Add(employee);
            }
            return new SuccessResult("Çalışanlar başarıyla eklendi.");
        }

        public IResult AssignRole(string employeeId, string role)
        {
            var emp = _employeeDal.Get(x=>x.Id == employeeId);
            if (emp == null)
            {
                return new ErrorResult("Çalışan bulunamadı.");
            }
            if(emp.Role == role)
            {
                return new ErrorResult("Çalışan zaten bu rolde.");
            }
            emp.Role = role;
            emp.LastUpdated = DateTime.Now;
            _employeeDal.Update(emp);
            return new SuccessResult("Çalışan rolü güncellendi.");
        }

        public IResult AssignToDepartment(string employeeId, string departmentId)
        {
            var emp = _employeeDal.Get(x => x.Id == employeeId);
            if (emp == null)
            {
                return new ErrorResult("Çalışan bulunamadı.");
            }
            emp.DeparmentId = departmentId;
            emp.LastUpdated = DateTime.Now;
            _employeeDal.Update(emp);
            return new SuccessResult("Çalışan departmanı güncellendi.");

        }

        public IResult Delete(string id)
        {
            _employeeDal.Delete(id);
            return new SuccessResult("Silindi.");
        }

        public IDataResult<List<Employee>> GetAll()
        {
            var allEmps = _employeeDal.GetAll();
            return new SuccessDataResult<List<Employee>>(allEmps);
        }

        public IDataResult<List<Employee>> GetByDepartmentId(string departmentId)
        {
            var emps = _employeeDal.GetAll(x=>x.DeparmentId == departmentId);

            return new SuccessDataResult<List<Employee>>(emps);
        }

        public IDataResult<Employee> GetById(string id)
        {
            var emp = _employeeDal.Get(x => x.Id == id);
            if (emp == null)
            {
                return new ErrorDataResult<Employee>(emp,"Çalışan bulunamadı.");
            }
            return new SuccessDataResult<Employee>(emp);
        }

        public IDataResult<List<Employee>> GetByRole(string role)
        {
            var emp = _employeeDal.GetAll(x => x.Role== role);
            if (emp == null)
            {
                return new ErrorDataResult<List<Employee>>(emp, "Çalışan bulunamadı.");
            }
            return new SuccessDataResult<List<Employee>>(emp);
        }

        public IDataResult<int> GetCountByDepartment(string departmentId)
        {

            var count = _employeeDal.GetAll(x => x.DeparmentId == departmentId).Count();

            return new SuccessDataResult<int>(count);
        }

        public IDataResult<int> GetCountByRole(string role)
        {
            var count = _employeeDal.GetAll(x => x.Role == role).Count();

            return new SuccessDataResult<int>(count);
        }



        public IDataResult<Employee> GetManagerByDepartment(string departmentId)
        {
            var department= _departmentDal.Get(x=>x.Id ==departmentId);
            var manager = _employeeDal.Get(x=>x.Id == department.ManagerId);

            return new SuccessDataResult<Employee>(manager);
        }

        public IDataResult<int> GetTotalEmployeeCount()
        {
            var count = _employeeDal.GetAll().Count();

            return new SuccessDataResult<int>(count);
        }


        public IResult Update(EmployeeDto employee, string employeeId)
        {
            var existingEmployee = _employeeDal.Get(x => x.Id == employeeId);
            if (existingEmployee == null)
            {
                return new ErrorResult("Güncellenecek çalışan bulunamadı.");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Surname = employee.Surname;
            existingEmployee.Role = employee.Role;
            existingEmployee.DeparmentId = employee.DeparmentId;
            existingEmployee.LastUpdated = DateTime.Now;
            _employeeDal.Update(existingEmployee);

            return new SuccessResult("Çalışan başarıyla güncellendi.");
        }

        public IResult UpdateRange(List<Employee> employees)
        {
            if (employees == null || !employees.Any())
            {
                return new ErrorResult("Gönderilen çalışan listesi boş.");
            }

            var notFoundIds = new List<string>();

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
                existingEmployee.DeparmentId = employee.DeparmentId;
                existingEmployee.LastUpdated = DateTime.Now;

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
