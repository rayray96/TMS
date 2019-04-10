using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        void Update(int id, T item);
        void Delete(int id);

        T GetById(int id);
        T GetSingle(Expression<Func<T, bool>> whereCondition);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetSingleAsync(Expression<Func<T, bool>> whereCondition);
        Task<T> GetByIdAsync(int id);
    }
}
