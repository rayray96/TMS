using System;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<Person> People { get; }
        IRepository<Team> Teams { get; }

        void Save();
        Task SaveAsync();
    }
}
