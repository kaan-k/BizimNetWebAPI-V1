using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Settings; // ✅ Matches the entity namespace
using System.Collections.Generic;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAgGridSettingsDal : EfEntityRepositoryBase<AgGridSettings, BizimNetContext>, IAgGridSettingsDal
    {
        public EfAgGridSettingsDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
    }
}