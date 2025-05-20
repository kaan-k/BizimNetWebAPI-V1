using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IEmployeeService
    {
        IDataResult<Employee> GetById(string id);
        IDataResult<List<Employee>> GetAll();
        IResult Add(EmployeeDto employee);
        IResult Update(EmployeeDto employee, string employeeId);
        IResult Delete(string id);

        IDataResult<List<Employee>> GetByDepartmentId(string departmentId);
        IResult AssignToDepartment(string employeeId, string departmentId);

        IDataResult<List<Employee>> GetByRole(string role);
        IDataResult<Employee> GetManagerByDepartment(string departmentId);
        IResult AssignRole(string employeeId, string role);


        IDataResult<int> GetTotalEmployeeCount();
        IDataResult<int> GetCountByRole(string role);
        IDataResult<int> GetCountByDepartment(string departmentId);

        IResult AddRange(List<EmployeeDto> employees);
        IResult UpdateRange(List<Employee> employees);
    }
}
