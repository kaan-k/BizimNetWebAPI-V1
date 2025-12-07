using Core.Utilities.Results;
using Entities.Concrete.Duties;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDutyService
    {
        // CRUD
        IDataResult<DutyDto> Add(DutyDto request);
        IDataResult<DutyDto> AddCompleted(DutyDto request);

        IResult Delete(int id);
        IResult Update(DutyDto request);

        // LIST / QUERY
        IDataResult<List<DutyDto>> GetAll();
        IDataResult<List<DutyDto>> GetAllDetails(int userId);
        IDataResult<List<DutyDto>> GetTodaysDuties();
        IDataResult<List<DutyDto>> GetAllByCustomerIdReport(int customerId);
        IDataResult<List<DutyDto>> ReplaceCustomerId(int customerId, int customerIdToReplace);
        IDataResult<List<DutyDto>> GetAllByCustomerId(int customerId);
        IDataResult<List<DutyDto>> GetAllByEmployeeId(int employeeId);
        IDataResult<List<DutyDto>> GetAllByStatus(string status);

        // SINGLE
        IDataResult<DutyDto> UpdateStatusById(int id, string newStatus);
        IDataResult<DutyDto> GetById(int id);
        IDataResult<DutyDto> MarkAsCompleted(int id);
    }
}
