using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete.Offers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOfferDal : EfEntityRepositoryBase<Offer, BizimNetContext>, IOfferDal
    {
        private readonly BizimNetContext _context;

        public EfOfferDal(BizimNetContext context) : base(context)
        {
            _context = context;
        }

        public List<Offer> GetAllOfferDetails()
        {
            return _context.Offers
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ToList();
        }

        public List<Offer> GetByStatus(string status)
        {
            return _context.Offers
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .Where(o => o.Status == status)
                .ToList();
        }

        public Offer GetByIdWithDetails(int id)
        {
            return _context.Offers
                .AsSplitQuery()
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

    }
}
