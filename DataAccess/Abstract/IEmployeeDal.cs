using Core.DataAccess; // ✅ Where IEntityRepository is located
using Entities.Concrete.Employees; // ✅ Use the fixed Namespace

namespace DataAccess.Abstract
{
    // 1. Inherit from IEntityRepository, not IMongoRepository
    public interface IEmployeeDal : IEntityRepository<Employee>
    {
    }
}