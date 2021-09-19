using basicbanking.api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace basicbanking.api.Data
{
    public interface IRepository<T> where T : EntityBase
    {
        
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        T GetById(long id);
        IRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> path);

    }
}