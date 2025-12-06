using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Departments;
using Entities.Concrete.Employees;
using System;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private readonly IDepartmentDal _departmentDal;
        private readonly IEmployeeDal _employeeDal;

        public DepartmentManager(IDepartmentDal departmentDal, IEmployeeDal employeeDal)
        {
            _departmentDal = departmentDal;
            _employeeDal = employeeDal;
        }

        public IDataResult<Department> Add(DepartmentDto departmentDto)
        {
            var departmentToAdd = new Department
            {
                // SQL Auto-Generates Id, do not set it here
                ManagerId = departmentDto.ManagerId,
                CreatedAt = DateTime.UtcNow, // ✅ Use UtcNow
                Name = departmentDto.Name,
            };

            _departmentDal.Add(departmentToAdd);
            return new SuccessDataResult<Department>(departmentToAdd, "Başarılı");
        }

        public IResult AssignManager(int departmentId, int employeeId)
        {
            var department = _departmentDal.Get(x => x.Id == departmentId);
            if (department == null)
            {
                return new ErrorResult("Departman bulunamadı.");
            }

            // Optional: Check if employee exists
            var employee = _employeeDal.Get(x => x.Id == employeeId);
            if (employee == null) return new ErrorResult("Çalışan bulunamadı.");

            department.ManagerId = employeeId;
            department.UpdatedAt = DateTime.UtcNow;

            _departmentDal.Update(department);

            return new SuccessResult("Yönetici başarıyla atandı.");
        }

        public IResult Delete(int id)
        {
            _departmentDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Department>> GetAll()
        {
            var departments = _departmentDal.GetAll();
            return new SuccessDataResult<List<Department>>(departments);
        }

        public IDataResult<Department> GetById(int id)
        {
            var department = _departmentDal.Get(x => x.Id == id);
            return new SuccessDataResult<Department>(department);
        }

        public IDataResult<int> GetEmployeeCount(int departmentId)
        {
            // ✅ Fixed Typo: DepartmentId (not DeparmentId)
            var employees = _employeeDal.GetAll(x => x.DepartmentId == departmentId);

            return new SuccessDataResult<int>(employees.Count, "Çalışan sayısı getirildi.");
        }

        public IDataResult<List<Employee>> GetEmployeesByDepartmentId(int departmentId)
        {
            var employees = _employeeDal.GetAll(x => x.DepartmentId == departmentId);
            return new SuccessDataResult<List<Employee>>(employees, "Bu departmandaki çalışanlar getirildi.");
        }

        public IDataResult<List<Employee>> GetEmployeesByRole(int departmentId, string role)
        {
            var employees = _employeeDal.GetAll(x => x.DepartmentId == departmentId && x.Role == role);
            return new SuccessDataResult<List<Employee>>(employees, $"Bu departmandaki {role} görevindeki çalışanlar getirildi.");
        }

        public IDataResult<Employee> GetManagerOfDepartment(int departmentId)
        {
            var department = _departmentDal.Get(x => x.Id == departmentId);
            if (department == null) return new ErrorDataResult<Employee>("Departman bulunamadı");

            // ManagerId is now an int, matching Employee.Id
            var manager = _employeeDal.Get(x => x.Id == department.ManagerId);

            return new SuccessDataResult<Employee>(manager, "Departman yöneticisi getirildi.");
        }

        public IResult IsDepartmentNameTaken(string name)
        {
            var department = _departmentDal.Get(x => x.Name == name);

            if (department == null)
            {
                return new SuccessResult("Departman adı kullanılabilir.");
            }
            return new ErrorResult("Bu departman zaten mevcut");
        }

        public IDataResult<List<Department>> SearchByName(string name)
        {
            // Simple SQL LIKE search
            var result = _departmentDal.GetAll(x => x.Name.Contains(name));
            return new SuccessDataResult<List<Department>>(result);
        }

        public IResult Update(DepartmentDto departmentDto)
        {
            // Assuming DTO has the ID
            var existingDepartment = _departmentDal.Get(x => x.Id == departmentDto.Id);
            if (existingDepartment == null)
            {
                return new ErrorResult("Güncellenecek departman bulunamadı.");
            }

            existingDepartment.Name = departmentDto.Name;
            existingDepartment.ManagerId = departmentDto.ManagerId;
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            _departmentDal.Update(existingDepartment);

            return new SuccessResult("Departman başarıyla güncellendi.");
        }
    }
}