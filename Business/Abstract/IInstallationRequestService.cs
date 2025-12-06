using Core.Utilities.Results;
using Entities.Concrete.InstallationRequests; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IInstallationRequestService
    {
        IResult Add(InstallationRequestDto request);

        // ✅ Changed string -> int
        IResult Delete(int id);
        IResult Update(InstallationRequest request);

        // ✅ Changed string -> int
        IResult AssignEmployee(int requestId, int employeeId);
        IResult UpdateNote(int requestId, string note);
        IResult MarkAsCompleted(int requestId);
        IResult SendInstallationMail(InstallationRequestDto request);
        IResult SendAssignmentMail();

        // ✅ Changed string -> int
        IDataResult<InstallationRequest> GetById(int id);
        IDataResult<List<InstallationRequest>> GetByCustomerId(int customerId);
        IDataResult<List<InstallationRequest>> GetByOfferId(int offerId);
        IDataResult<List<InstallationRequest>> GetUnassigned();
        IDataResult<List<InstallationRequest>> GetAssigned();
        IDataResult<List<InstallationRequest>> GetAll();
        IDataResult<List<InstallationRequest>> GetAllInstallationRequestDetails();

        IDataResult<List<InstallationRequest>> WorkerCalculateEscalation();
    }
}