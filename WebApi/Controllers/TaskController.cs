using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using WebApi.Configurations;
using WebApi.Filters;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IStatusService statusService;
        private readonly IPersonService personService;
        private readonly IMapper mapper;

        public TaskController(ITaskService taskService, IPersonService personService, IStatusService statusService)
        {
            this.taskService = taskService;
            this.personService = personService;
            this.statusService = statusService;

            mapper = MapperConfig.GetMapperResult();
        }

        // GET api/task/statuses
        [Authorize(Roles = "Worker, Manager")]
        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            IEnumerable<StatusViewModel> statuses;
            if (User.IsInRole("Worker"))
            {
                statuses = mapper.Map<IEnumerable<StatusDTO>, IEnumerable<StatusViewModel>>(statusService.GetStatusesForAssignee());
            }
            else
            {
                statuses = mapper.Map<IEnumerable<StatusDTO>, IEnumerable<StatusViewModel>>(statusService.GetStatusesForAuthor());
            }

            return Ok(statuses);
        }

        // GET api/task/workerTasks/{id}
        [Authorize(Roles = "Worker")]
        [UserValidationActionFilter]
        [HttpGet("workerTasks/{id}")]
        public IActionResult GetTasksOfAssignee(string id)
        {
            var myTasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetTasksOfAssignee(id));
            if (myTasks == null)
            {
                ModelState.AddModelError("", "Cannot find the tasks of the current worker!");
                Log.Warning($"Cannot find the tasks of the current worker with UserId: {id}");
                return BadRequest(ModelState);
            }

            return Ok(myTasks);
        }

       
        // GET api/task/managerTasks/{id}
        [Authorize(Roles = "Manager")]
        [UserValidationActionFilter]
        [HttpGet("managerTasks/{id}")]
        public IActionResult GetTasksOfAuthor(string id)
        {
            var myTasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetTasksOfAuthor(id));
            if (myTasks == null)
            {
                ModelState.AddModelError("", "Cannot find the tasks of the current manager!");
                Log.Warning($"Cannot find the tasks of the current manager with UserId: {id}");
                return BadRequest(ModelState);
            }

            return Ok(myTasks);
        }
      
        // POST api/task
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult CreateTask([FromBody]EditTaskViewModel newTask)
        {
            string author = HttpContext.User.Identity.Name;

            taskService.CreateTask(mapper.Map<EditTaskViewModel, EditTaskDTO>(newTask), author, newTask.AssigneeId, newTask.Priority);

            Log.Information($"Task was been created by the manager with UserName: {author}");
            return Ok(new { message = "Task has created!" });
        }

        // PUT api/task/{id}
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody]EditTaskViewModel taskUpdate)
        {
            string author = HttpContext.User.Identity.Name;
            var task = mapper.Map<EditTaskViewModel, EditTaskDTO>(taskUpdate);

            taskService.UpdateTask(task, id, author, taskUpdate.AssigneeId, taskUpdate.Priority);

            Log.Information($"Task with Id: {id} was been updated by the manager with UserName: {author}");
            return Ok(new { message = "Task has changed" });
        }

    
        // PUT api/task/{id}/status
        [Authorize(Roles = "Worker, Manager")]
        [UserValidationActionFilter]
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(string id, [FromBody]EditStatusViewModel status)
        {
            PersonDTO person = personService.GetPerson(id);

            if (status != null)
            {
                taskService.UpdateStatus(status.TaskId, status.Status, person.Id);
                Log.Information($"Status was been updated by the user with UserId: {id}");
            }

            return Ok();
        }

        // DELETE api/task/{id}
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            string author = HttpContext.User.Identity.Name;

            taskService.DeleteTask(id, author);

            Log.Information($"Task with Id: {id} was been deleted by the manager with UserId: {author}");
            return Ok(new { message = "Task has deleted" });
        }
    }
}
