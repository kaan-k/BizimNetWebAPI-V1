using Core.DataAccess; // ✅ Where IEntityRepository is located
using Entities.Concrete.Stocks; // ✅ Use the fixed Plural Namespace

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface IStockDal : IEntityRepository<Stock>
    {
    }
}