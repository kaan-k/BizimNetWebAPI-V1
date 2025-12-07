using Core.DataAccess;
using Entities.Concrete.Settings;
using Entities.Concrete.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IWarehouseDal: IEntityRepository<Warehouse>
    {
        List<Warehouse> GetAllWithStocks();
        Warehouse GetByIdWithStocks(int id);

    }
}
