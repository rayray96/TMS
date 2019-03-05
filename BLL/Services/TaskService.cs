using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private IUnitOfWork db { get; set; }

        private IMapper mapper { get; set; }

        public TaskService(IUnitOfWork uow)
        {
            db = uow;

            var config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<ApplicationUser, UserDTO>();
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<TaskInfo, TaskDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<Priority, PriorityDTO>();
            });

            mapper = config.CreateMapper();
        }

        public TaskDTO GetTask(int id)
        {
            var result = db.Tasks.GetById(id);
            return mapper.Map<TaskInfo, TaskDTO>(result);
        }
    }
}
