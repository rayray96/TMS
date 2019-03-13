using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Configurations
{
    public static class MapperConfig
    {
        public static IMapper GetMapperResult()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<Priority, PriorityDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<TaskInfo, TaskDTO>();
            });

            return config.CreateMapper();
        }

        public static IMapper GetIdentityMapperResult()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<ApplicationUser, UserDTO>();
                cfg.CreateMap<RefreshToken, RefreshTokenDTO>();
            });

            return config.CreateMapper();
        }
    }
}
