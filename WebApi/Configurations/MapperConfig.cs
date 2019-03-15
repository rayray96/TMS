using AutoMapper;
using BLL.DTO;
using WebApi.Models;

namespace WebApi.Configurations
{
    public static class MapperConfig
    {
        public static IMapper GetMapperResult()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonDTO, PersonViewModel>();
                cfg.CreateMap<StatusDTO, StatusViewModel>();
                cfg.CreateMap<PriorityDTO, PriorityViewModel>();
                cfg.CreateMap<TeamDTO, TeamViewModel>();
                cfg.CreateMap<TaskDTO, TaskViewModel>();
            });

            return config.CreateMapper();
        }
    }
}
