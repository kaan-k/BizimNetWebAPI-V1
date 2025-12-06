using Core.DataAccess;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        protected readonly TContext _context;

        public EfEntityRepositoryBase(TContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        // Helper: delete by id
        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().SingleOrDefault(filter);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return query.ToList();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            var entities = _context.Set<TEntity>().Where(filter).ToList();

            _context.Set<TEntity>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public List<TEntity> GetAllWithPage(int page, int limit)
        {
            return _context.Set<TEntity>()
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
        }
    }
}
