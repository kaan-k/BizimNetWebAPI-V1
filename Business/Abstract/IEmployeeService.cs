using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.Employees; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IEmployeeService
    {
        // ✅ Changed string -> int
        IDataResult<Employee> GetById(int id);
        IDataResult<List<Employee>> GetAll();
        IResult Add(EmployeeDto employee);

        // ✅ Changed string -> int
        IResult Update(EmployeeDto employee, int employeeId);
        IResult Delete(int id);

        IDataResult<List<Employee>> GetByDepartmentId(int departmentId);
        IResult AssignToDepartment(int employeeId, int departmentId);

        IDataResult<List<Employee>> GetByRole(string role);
        IDataResult<Employee> GetManagerByDepartment(int departmentId);
        IResult AssignRole(int employeeId, string role);

        IResult ReturnEmployeeEmail(int employeeId);

        IDataResult<int> GetTotalEmployeeCount();
        IDataResult<int> GetCountByRole(string role);
        IDataResult<int> GetCountByDepartment(int departmentId);

        IResult AddRange(List<EmployeeDto> employees);
        IResult UpdateRange(List<Employee> employees);
    }
}