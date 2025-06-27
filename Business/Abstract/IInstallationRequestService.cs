using Core.Utilities.Results;
using Entities.Concrete.InstallationRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IInstallationRequestService
    {
        IResult Add(InstallationRequestDto request);
        IResult Delete(string id);
        IResult Update(InstallationRequest request);
        IResult AssignEmployee(string requestId, string employeeId);
        IResult UpdateNote(string requestId, string note);
        IResult MarkAsCompleted(string requestId);
        IResult SendInstallationMail(InstallationRequestDto request);
        IResult SendAssignmentMail();

        IDataResult<InstallationRequest> GetById(string id);
        IDataResult<List<InstallationRequest>> GetByCustomerId(string customerId);
        IDataResult<List<InstallationRequest>> GetByOfferId(string offerId);
        IDataResult<List<InstallationRequest>> GetUnassigned();
        IDataResult<List<InstallationRequest>> GetAssigned();
        IDataResult<List<InstallationRequest>> GetAll();
        IDataResult<List<InstallationRequest>> WorkerCalculateEscalation();
    }
}
