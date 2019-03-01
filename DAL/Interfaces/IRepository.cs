using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(int id);

        T Get(int id);

        IEnumerable<T> Find(Func<T, Boolean> predicate);
        IEnumerable<T> GetAll();
    }
}
