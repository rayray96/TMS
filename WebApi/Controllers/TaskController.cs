using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Configurations;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
        [HttpGet("workerTasks/{id}")]
        public IActionResult GetTasksOfAssignee(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonDTO worker = personService.GetPerson(id);

            var myTasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetTasksOfAssignee(id));
            if (myTasks == null)
            {
                ModelState.AddModelError("", "Cannot find the tasks of current worker!");
                return BadRequest(ModelState);
            }

            return Ok(myTasks);
        }
        // GET api/task/managerrTasks/{id}
        [Authorize(Roles = "Manager")]
        [HttpGet("managerTasks/{id}")]
        public IActionResult GetTasksOfAuthor(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonDTO manager = personService.GetPerson(id);

            var myTasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetTasksOfAuthor(id));
            if (myTasks == null)
            {
                ModelState.AddModelError("", "Cannot find the tasks of current manager!");
                return BadRequest(ModelState);
            }

            return Ok(myTasks);
        }
        // POST api/task
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult CreateTask([FromBody]EditTaskViewModel newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string author = HttpContext.User.Identity.Name;

            taskService.CreateTask(mapper.Map<EditTaskViewModel, EditTaskDTO>(newTask), author, newTask.AssigneeId, newTask.Priority);

            return Ok(new { message = "Task has created!" });
        }
        // PUT api/task/{id}
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody]EditTaskViewModel taskUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string author = HttpContext.User.Identity.Name;

            var task = mapper.Map<EditTaskViewModel, EditTaskDTO>(taskUpdate);

            taskService.UpdateTask(task, id, author, taskUpdate.AssigneeId, taskUpdate.Priority);

            return Ok(new { message = "Task has changed" });
        }
        // PUT api/task/{id}/status
        [Authorize(Roles = "Worker, Manager")]
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(string id, [FromBody]EditStatusViewModel status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonDTO person = personService.GetPerson(id);

            if (status != null)
            {
                taskService.UpdateStatus(status.TaskId, status.Status, person.Id);
            }

            return Ok();
        }
        // DELETE api/task/{id}
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string author = HttpContext.User.Identity.Name;

            taskService.DeleteTask(id, author);

            return Ok(new { message = "Task has deleted" });
        }
    }
}
