using System;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IPersonRepository : IRepository<Person>, IDisposable 
    {
        void Create(Person person, string teamName);
    }
}
