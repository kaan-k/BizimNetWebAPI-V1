using Core.Utilities.Results;
using Entities.Concrete.Stocks;
using Entities.DTOs;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IStockGroupService
    {
        IDataResult<StockGroup> Add(StockGroupAddDto stockGroupAddDto);
        IResult Update(StockGroup stockGroup);
        IResult Delete(int id);
        IDataResult<StockGroupAddDto> GetById(int id);
        IDataResult<List<StockGroup>> GetAll();
    }
}