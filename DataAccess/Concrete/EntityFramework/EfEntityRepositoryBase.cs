using Core.DataAccess;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    // TEntity: The class (e.g., Product, Customer)
    // TContext: Your DbContext (e.g., BizimNetContext)
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public void Add(TEntity entity)
        {
            // 'using' block ensures the context is disposed after the operation
            using (var context = new TContext())
            {
                var addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        // Helper overload if you just want to delete by ID
        public void Delete(int id)
        {
            using (var context = new TContext())
            {
                // We find the entity first, then delete it
                var entity = context.Set<TEntity>().Find(id);
                if (entity != null)
                {
                    context.Set<TEntity>().Remove(entity);
                    context.SaveChanges();
                }
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                // 1. Find all entities that match the filter
                var entities = context.Set<TEntity>().Where(filter).ToList();

                // 2. Remove them all
                context.Set<TEntity>().RemoveRange(entities);

                // 3. Save
                context.SaveChanges();
            }
        }

        public List<TEntity> GetAllWithPage(int page, int limit)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>()
                    .Skip((page - 1) * limit) // Skip previous pages
                    .Take(limit)              // Take (Limit) the next set
                    .ToList();
            }
        }
    }
}