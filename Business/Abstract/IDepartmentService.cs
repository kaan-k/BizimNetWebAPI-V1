using Core.Utilities.Results;
using Entities.Concrete.Departments; // ✅ Plural Namespace
using Entities.Concrete.Employees;   // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDepartmentService
    {
        IDataResult<Department> Add(DepartmentDto department);

        // ✅ Changed string -> int
        IResult Update(DepartmentDto department);
        IResult Delete(int id);

        IDataResult<Department> GetById(int id);
        IDataResult<List<Department>> GetAll();
        IDataResult<List<Department>> SearchByName(string name);

        // ✅ Changed string -> int
        IDataResult<List<Employee>> GetEmployeesByDepartmentId(int departmentId);
        IDataResult<int> GetEmployeeCount(int departmentId);
        IResult IsDepartmentNameTaken(string name);
        IDataResult<Employee> GetManagerOfDepartment(int departmentId);
        IResult AssignManager(int departmentId, int employeeId);
        IDataResult<List<Employee>> GetEmployeesByRole(int departmentId, string role);
    }
}