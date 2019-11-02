using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Configurations.FactoryMethod
{
    public class CommonMapper : IWrappedMapper
    {
        public IMapper CreateMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<Priority, PriorityDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<TaskInfo, TaskDTO>();
                cfg.CreateMap<TaskInfo, EditTaskDTO>();

                cfg.CreateMap<EditTaskDTO, TaskInfo>();
                cfg.CreateMap<PersonDTO, Person>();
                cfg.CreateMap<StatusDTO, Status>();
                cfg.CreateMap<PriorityDTO, Priority>();
                cfg.CreateMap<TeamDTO, Team>();
                cfg.CreateMap<TaskDTO, TaskInfo>();
            });

            return config.CreateMapper();
        }
    }
}
