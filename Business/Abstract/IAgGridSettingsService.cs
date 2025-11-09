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
        IDataResult<AgGridSettings> Add(AgGridSettingsDto agGridSetting);
        IResult Update(AgGridSettings agGridSetting, string id);
        IResult Delete(string id);
        IDataResult<AgGridSettings> GetById(string id);
        IDataResult<List<AgGridSettings>> GetAll();
    }
}
