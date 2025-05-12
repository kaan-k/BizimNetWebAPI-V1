using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess.MongoDB
{
    public interface IMongoRepository<T>
    {
        void Add(T entity);
        T Get(Expression<Func<T, bool>> filter);
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        void Update(T entity);
        void Delete(string id);
        void DeleteMany(Expression<Func<T, bool>> filter);
        List<T> GetAllWithPage(int page, int limit);
    }
}
