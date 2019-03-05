using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IRepository<Person> People { get; }

        void Save();
        Task SaveAsync();
    }
}
