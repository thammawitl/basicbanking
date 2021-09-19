using basicbanking.api.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace basicbanking.api.Data
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private List<string> includePaths = new List<string>();
        protected readonly DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
        }

        public virtual void Insert(T entity)
        {
            this.context.Set<T>().Add(entity);
            this.context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            this.context.Attach<T>(entity);
            this.context.Entry<T>(entity).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
            this.context.SaveChanges();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            var query = this.context.Set<T>().AsQueryable();

            foreach (var inc in this.includePaths)
                query = query.Include(inc);

            return query.Where(expression);
        }

        public T GetById(long id)
        {
            var query = this.Find(i => i.Id == id);

            foreach (var inc in this.includePaths)
                query = query.Include(inc);

            return query.FirstOrDefault();
        }

        public IRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> path)
        {
            var expression = (MemberExpression)path.Body;
            string name = expression.Member.Name;
            this.includePaths.Add(name);
            return this;
        }
    }
}
