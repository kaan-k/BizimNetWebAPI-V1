using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Stocks; // ✅ Use the fixed Plural Namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfStockDal : EfEntityRepositoryBase<Stock, BizimNetContext>, IStockDal
    {
        // No constructor needed. Base class handles the Context.
    }
}