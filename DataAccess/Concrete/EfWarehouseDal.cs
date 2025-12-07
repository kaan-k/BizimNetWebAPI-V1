using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete.Aggrements;
using Entities.Concrete.Warehouses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfWarehouseDal : EfEntityRepositoryBase<Warehouse, BizimNetContext>, IWarehouseDal
    {
        public EfWarehouseDal(BizimNetContext context) : base(context)
        {
        }

        public List<Warehouse> GetAllWithStocks()
        {
            using (var context = new BizimNetContext())
            {
                return context.Warehouses
                    .Include(w => w.Stocks)
                    .ToList();
            }
        }
        public Warehouse GetByIdWithStocks(int id)
        {
            using (var context = new BizimNetContext())
            {
                return context.Warehouses
                    .Include(w => w.Stocks)
                    .FirstOrDefault(w => w.Id == id);
            }
        }


    }
}
