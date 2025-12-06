using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Devices; // ✅ Use the fixed Plural Namespace
using Microsoft.EntityFrameworkCore; // ✅ Needed for .Include()
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfDeviceDal : EfEntityRepositoryBase<Device, BizimNetContext>, IDeviceDal
    {
        public EfDeviceDal(BizimNetContext context) : base(context)
        {
            // Any specific logic for EfOfferDal can go here, but usually it's left empty.
        }
        public List<Device> GetAllDeviceDetails()
        {
            using (var context = new BizimNetContext())
            {
                // ✅ SQL Way:
                // This fetches the Device AND the related Customer in one query.
                // Access the name via: device.Customer.CompanyName
                return context.Devices // Ensure DbSet is named 'Devices' in Context
                    .Include(d => d.Customer)
                    .ToList();
            }
        }
    }
}