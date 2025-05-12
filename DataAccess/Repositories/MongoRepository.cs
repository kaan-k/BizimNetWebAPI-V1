using Core.DataAccess;
using Core.DataAccess.MongoDB;
using Core.Entities;
using Core.Entities.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : IEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public void Add(T entity)
        {
            _collection.InsertOne(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _collection.Find(filter).FirstOrDefault();
        }

        public List<T> GetAll(Expression<Func<T, bool>> filter = null)
        {
            return filter == null
                ? _collection.Find(_ => true).ToList()
                : _collection.Find(filter).ToList();
        }

        public void Update(T entity)
        {
            _collection.ReplaceOne(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity);
        }

        public void Delete(string id)
        {
            _collection.DeleteOne(Builders<T>.Filter.Eq(e => e.Id, id));
        }

        public void DeleteMany(Expression<Func<T, bool>> filter)
        {
            _collection.DeleteMany(filter);
        }

        public List<T> GetAllWithPage(int page, int limit)
        {
            return _collection.Find(_ => true)
                .Skip((page - 1) * limit)
                .Limit(limit)
                .ToList();
        }
    }
}
