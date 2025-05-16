using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDepartmentService
    {
        IDataResult<Department> Add(DepartmentDto department);
        IResult Update(Department department, string departmentId);
        IResult Delete(string id);
        IDataResult<Department> GetById(string id);
        IDataResult<List<Department>> GetAll();
        IDataResult<List<Department>> SearchByName(string name);
        IDataResult<List<Employee>> GetEmployeesByDepartmentId(string departmentId);
        IDataResult<int> GetEmployeeCount(string departmentId);
        IResult IsDepartmentNameTaken(string name);
        IDataResult<Employee> GetManagerOfDepartment(string departmentId);
        IResult AssignManager(string departmentId, string employeeId);
        IDataResult<List<Employee>> GetEmployeesByRole(string departmentId, string role);
    }
}
