using Core.DataAccess; // ✅ Where IEntityRepository is located
using Entities.Concrete.Settings; // ✅ Matches the entity namespace

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface IAgGridSettingsDal : IEntityRepository<AgGridSettings>
    {
    }
}