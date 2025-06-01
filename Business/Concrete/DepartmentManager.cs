using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IDataResult<Department> Add(DepartmentDto department)
        {
            var departmentToAdd = new Department
            {
                ManagerId = department.ManagerId,
                CreatedAt = DateTime.Now,
                Name = department.Name,
            };
            _departmentDal.Add(departmentToAdd);
            return new SuccessDataResult<Department>(departmentToAdd, "başarılı");
        }

        public IResult AssignManager(string departmentId, string employeeId)
        {
            var department = _departmentDal.Get(x=> x.Id == departmentId);
            //emp dal ile emp sorgusu

            //emp.DepartmentId == departmentId sorugus

            if (department == null)
            {
                return new ErrorResult("Departman bulunamadı.");
            }
            department.ManagerId = employeeId;
            department.UpdatedAt = DateTime.Now;
            _departmentDal.Update(department);

            return new SuccessResult("Yönetici başarıyla atandı.");
        }

        public IResult Delete(string id)
        {
            _departmentDal.Delete(id);
            return new SuccessResult();
        }

        public IDataResult<List<Department>> GetAll()
        {
            var departments = _departmentDal.GetAll();
            return new SuccessDataResult<List<Department>>(departments);
        }

        public IDataResult<Department> GetById(string id)
        {
            var department = _departmentDal.Get(x=> x.Id == id);
            return new SuccessDataResult<Department>(department);
        }

        public IDataResult<int> GetEmployeeCount(string departmentId)
        {
            var employees = _employeeDal.GetAll(x => x.DeparmentId == departmentId);

            return new SuccessDataResult<int>(employees.Count, "Çalışan sayısı getirildi.");
        }

        public IDataResult<List<Employee>> GetEmployeesByDepartmentId(string departmentId)
        {
            var employees = _employeeDal.GetAll(x => x.DeparmentId == departmentId);
            return new SuccessDataResult<List<Employee>>(employees, "Bu departmandaki çalışanlar getirildi.");
        }

        public IDataResult<List<Employee>> GetEmployeesByRole(string departmentId, string role)
        {
            var employees = _employeeDal.GetAll(x => x.DeparmentId == departmentId && x.Role == role);
            return new SuccessDataResult<List<Employee>>(employees, $"Bu departmandaki {role} görevindeki çalışanlar getirildi.");
        }

        public IDataResult<Employee> GetManagerOfDepartment(string departmentId)
        {
            var department = _departmentDal.Get(x => x.Id == departmentId);

            var manager = _employeeDal.Get(x => x.Id == department.ManagerId);

            return new SuccessDataResult<Employee>(manager, "Departman yöneticisi getirildi.");
        }

        public IResult IsDepartmentNameTaken(string name)
        {
            var departments = _departmentDal.Get(x=>x.Name == name);

            if(departments == null)
            {
                return new SuccessResult("Departman yok.");
            }
            return new ErrorResult("Bu departman zaten mevcut");

        }

        public IDataResult<List<Department>> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public IResult Update(Department department, string departmentId)
        {
            var existingDepartment = _departmentDal.Get(x => x.Id == departmentId);
            if (existingDepartment == null)
            {
                return new ErrorResult("Güncellenecek departman bulunamadı.");
            }

            existingDepartment.Name = department.Name;
            existingDepartment.ManagerId = department.ManagerId;
            existingDepartment.UpdatedAt = DateTime.Now;
            _departmentDal.Update(existingDepartment);

            return new SuccessResult("Departman başarıyla güncellendi.");
        }
    }
}
