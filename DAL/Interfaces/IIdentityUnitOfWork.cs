using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }

        //Task SaveAsync();
    }
}
