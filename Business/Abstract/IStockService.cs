using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Service;
using Entities.Concrete.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStockService
    {
        IDataResult<StockAddDto> Add(StockAddDto stock);
        IDataResult<Stock> Update(Stock servicing);
        IDataResult<List<Stock>> GetByDeviceType(DeviceType devicetype);
        IResult Delete(string id);
        IDataResult<List<Stock>> GetAll();
    }
}
