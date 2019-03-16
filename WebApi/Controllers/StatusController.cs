using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Configurations;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService statusService;
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public StatusController(IStatusService statusService, ITaskService taskService)
        {
            this.statusService = statusService;
            this.taskService = taskService;
            mapper = MapperConfig.GetMapperResult();
        }

        [HttpGet]
        public ActionResult GetStatuses()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<StatusViewModel> statuses;
            if (User.IsInRole("Worker"))
            {
                statuses = mapper.Map<IEnumerable<StatusDTO>, IEnumerable<StatusViewModel>>(statusService.GetActiveStatuses());
            }
            else
            {
                statuses = mapper.Map<IEnumerable<StatusDTO>, IEnumerable<StatusViewModel>>(statusService.GetNotActiveStatuses());
            }
            return Ok(statuses);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStatus(int id, [FromBody] string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIdInt = Convert.ToInt32(userIdString);
            if (status != null)
            {
                taskService.UpdateStatus(id, status, userIdInt);
            }

            return Ok();
        }
    }
}