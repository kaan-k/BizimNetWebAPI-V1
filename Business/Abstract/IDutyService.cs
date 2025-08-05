using Core.Utilities.Results;
using Entities.Concrete.Device;
using Entities.Concrete.Duty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDutyService
    {
        IDataResult<Duty> Add(DutyDto request);
        IResult Delete(string id);
        IResult Update(Duty request);
        IDataResult<List<Duty>> GetAll();
        IDataResult<List<Duty>> GetAllDetails();

        IDataResult<List<Duty>> GetAllByCustomerId(string customerId);
        IDataResult<List<Duty>> GetAllByStatus(string status);
        IDataResult<Duty> UpdateStatusById(string id, string newStatus);
        IDataResult<Duty> GetById(string id);


    }
}
