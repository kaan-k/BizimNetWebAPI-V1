using Core.Utilities.Results;
using Entities.Concrete.Services; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IServicingService
    {
        IDataResult<ServicingAddDto> Add(ServicingAddDto service);
        IDataResult<Servicing> Update(Servicing servicing);

        IDataResult<Servicing> GetByTrackingId(string trackingId);

        // ✅ Changed string -> int
        IDataResult<Servicing> GetById(int id);
        IResult MarkAsCompleted(int id);
        IResult MarkAsInProgress(int id);

        IResult SendCompletionMail(Servicing servicing);
        IDataResult<List<Servicing>> GetAll();
    }
}