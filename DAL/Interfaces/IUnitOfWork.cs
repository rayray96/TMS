using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Person> People { get; }
        IRepository<Priority> Priorities { get; }
        IRepository<Status> Statuses { get; }
        IRepository<TaskInfo> Tasks { get; }
        IRepository<Team> Teams { get; }

        void Save();
    }
}
