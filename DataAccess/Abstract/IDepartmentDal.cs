using Core.DataAccess; // ✅ Where IEntityRepository is located
using Entities.Concrete.Departments; // ✅ Use the fixed Plural Namespace

namespace DataAccess.Abstract
{
    // Switch inheritance from IMongoRepository to IEntityRepository
    public interface IDepartmentDal : IEntityRepository<Department>
    {
    }
}