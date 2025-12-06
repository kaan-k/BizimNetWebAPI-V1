using Core.Utilities.Results;
using Entities.Concrete.Duties; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IDutyService
    {
        IDataResult<Duty> Add(DutyDto request);
        IDataResult<Duty> AddCompleted(DutyDto request);

        // ✅ Changed string -> int
        IResult Delete(int id);
        IResult Update(Duty request);

        IDataResult<List<Duty>> GetAll();

        // ✅ Changed string -> int
        IDataResult<List<Duty>> GetAllDetails(int userId);

        IDataResult<List<Duty>> GetTodaysDuties();
        IDataResult<List<Duty>> GetAllByCustomerIdReport(int customerId);

        // ✅ Changed string -> int
        IDataResult<List<Duty>> ReplaceCustomerId(int customerId, int customerIdToReplace);
        IDataResult<List<Duty>> GetAllByCustomerId(int customerId);
        IDataResult<List<Duty>> GetAllByEmployeeId(int employeeId);

        IDataResult<List<Duty>> GetAllByStatus(string status);
        IDataResult<Duty> UpdateStatusById(int id, string newStatus);
        IDataResult<Duty> GetById(int id);
        IDataResult<Duty> MarkAsCompleted(int id);
    }
}