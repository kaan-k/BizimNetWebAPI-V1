using Core.Utilities.Results;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAgGridSettingsService
    {
        IDataResult<Payment> Add(AgGridSettingsDto agGridSetting);
        IResult Update(Payment agGridSetting, string id);
        IResult Delete(string id);
        IDataResult<Payment> GetById(string id);
        IDataResult<List<Payment>> GetAll();
    }
}
