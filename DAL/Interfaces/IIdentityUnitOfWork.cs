using System;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        PersonRepository People { get; }

        void Save();
        Task SaveAsync();
    }
}
