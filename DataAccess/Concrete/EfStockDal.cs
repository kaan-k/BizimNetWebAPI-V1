using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Stocks; // ✅ Use the fixed Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfStockDal : EfEntityRepositoryBase<Stock, BizimNetContext>, IStockDal
    {
        public EfStockDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
    }
}