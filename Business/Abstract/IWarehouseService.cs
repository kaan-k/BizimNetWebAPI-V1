using Core.Utilities.Results;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWarehouseService
    {
        IDataResult<Warehouse> Add(WarehouseAddDto warehouseAddDto);
        IResult Update(Warehouse warehouse);
        IResult Delete(int id);
        IDataResult<WarehouseAddDto> GetById(int id);
        IDataResult<List<Warehouse>> GetAll();
    }
}
