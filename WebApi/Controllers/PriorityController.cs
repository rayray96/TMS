using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Configurations;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityService priorityService;
        private readonly IMapper mapper;

        public PriorityController(IPriorityService priorityService)
        {
            this.priorityService = priorityService;
            mapper = MapperConfig.GetMapperResult();
        }

        [HttpGet]
        public IActionResult GetPriorities()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<PriorityViewModel> priorities = mapper.Map<IEnumerable<PriorityDTO>, IEnumerable<PriorityViewModel>>(priorityService.GetAllPriorities()).ToList();

            return Ok(priorities);
        }

        [HttpGet("{id}")]
        public IActionResult GetTasksWithPriority(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(priorityService.GetTaskWithPriority(id));

            return Ok(tasks);
        }
    }
}