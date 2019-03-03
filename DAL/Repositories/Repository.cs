using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public abstract class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected DbSet<T> Set => Context.Set<T>();
        protected ApplicationContext Context { get; }

        protected Repository(ApplicationContext context)
        {
            if (context == null)
                throw new ArgumentNullException("An instance of ApplicationContext is required to use this repository.", nameof(context));

            Context = context;
        }

        public T GetById(int id)
        {
            return Set.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Set.FindAsync(id);
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return Set.FirstOrDefault(whereCondition);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Set.FirstOrDefaultAsync(whereCondition);
        }

        public IEnumerable<T> GetAll()
        {
            return Set.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> whereCondition)
        {
            return Set.Where(whereCondition).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Set.Where(whereCondition).ToListAsync();
        }

        public IQueryable<T> GetQueryable()
        {
            return Set.AsQueryable();
        }

        public void Create(T item)
        {
            Set.Add(item);
        }

        public void Update(T item)
        {
            Set.Update(item);
        }

        public void Delete(T item)
        {
            Set.Remove(item);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
