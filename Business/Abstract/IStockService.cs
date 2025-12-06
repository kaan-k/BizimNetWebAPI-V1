using Core.Enums;
using Core.Utilities.Results;
using Entities.Concrete.Stocks; // ✅ Plural Namespace
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IStockService
    {
        IDataResult<StockAddDto> Add(StockAddDto stock);
        IDataResult<Stock> Update(Stock stock);
        IDataResult<List<Stock>> GetByDeviceType(DeviceType deviceType);

        // ✅ Changed string -> int
        IResult Delete(int id);

        IDataResult<List<Stock>> GetAll();
    }
}