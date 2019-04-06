using AutoMapper;
using BLL.DTO;
using BLL.Configurations;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class StatusService : IStatusService
    {
        private IUnitOfWork db { get; set; }
        private IMapper mapper { get; set; }

        public StatusService(IUnitOfWork uow)
        {
            db = uow;
            mapper = MapperConfig.GetMapperResult();
        }

        public IEnumerable<StatusDTO> GetAllStatuses()
        {
            var statuses = db.Statuses.GetAll();

            return mapper.Map<IEnumerable<Status>, IEnumerable<StatusDTO>>(statuses);
        }

        public IEnumerable<StatusDTO> GetStatusesForAssignee()
        {
            return GetAllStatuses().Where(s => s.Name != "Canceled" && s.Name != "Completed");
        }

        public IEnumerable<StatusDTO> GetStatusesForAuthor()
        {
            return GetAllStatuses().Where(s => s.Name == "Canceled" && s.Name == "Completed" && s.Name == "Not started");
        }
    }
}
