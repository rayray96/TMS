using System;
using System.Threading.Tasks;
using DAL.Identity;

namespace DAL.Interfaces
{
    public interface IdentityUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IPersonManager PersonManager { get; }

        Task SaveAsync();
    }
}
