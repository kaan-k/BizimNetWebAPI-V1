using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Aggrements;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAggrementDal : EfEntityRepositoryBase<Aggrement, BizimNetContext>, IAggrementDal
    {
        public EfAggrementDal(BizimNetContext context) : base(context)
        {
        }

        public Aggrement GetAllAgreementDetails(int id)
        {
            return _context.Aggrements
                .Include(a => a.Customer)
                .Include(a => a.Offer)
                .Include(a => a.Billings)
                .FirstOrDefault(a => a.Id == id);
        }
    }
}
