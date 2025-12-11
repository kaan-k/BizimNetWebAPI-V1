using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Stocks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfStockGroupDal : EfEntityRepositoryBase<StockGroup, BizimNetContext>, IStockGroupDal
    {
        public EfStockGroupDal(BizimNetContext context) : base(context)
        {
        }
    }
}