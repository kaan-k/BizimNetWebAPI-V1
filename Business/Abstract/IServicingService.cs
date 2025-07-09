using Autofac.Core;
using Core.Utilities.Results;
using Entities.Concrete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IServicingService
    {
        IDataResult<ServicingAddDto> Add(ServicingAddDto service);
        IDataResult<Servicing> Update(Servicing servicing);
        IDataResult<Servicing> GetByTrackingId(string trackingId);
        IResult MarkAsCompleted(string id);
        IResult MarkAsInProgress(string id);
        IResult SendCompletionMail(Servicing servicing);

    }
}
