using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Settings; // ✅ Matches the entity namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAgGridSettingsDal : EfEntityRepositoryBase<AgGridSettings, BizimNetContext>, IAgGridSettingsDal
    {
        // No constructor needed. Base class handles the Context.
    }
}