using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Configurations;
using BLL.DTO;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using System.Security.Principal;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IPersonService personService;
        private readonly IMapper mapper;

        public TaskController(ITaskService taskService, IPersonService personService)
        {
            this.taskService = taskService;
            this.personService = personService;
            mapper = MapperConfig.GetMapperResult();
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<TaskViewModel> tasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(
                taskService.GetTasksOfAssignee(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();

            return Ok(tasks);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Try to delete this task and check task id and current user UserName if he can delete it,
                // because deleting task is available only for author of task.
                taskService.DeleteTask(id, User.Identity.Name);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Task hasn't deleted.", e.Message);
                return BadRequest();
            }

            return Ok(new { result = "Task has deleted" });
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult CreateTask([FromBody]CreateTaskViewModel newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string author = HttpContext.User.Identity.Name;

            var task = new TaskViewModel
            {
                Name = newTask.Name,
                Description = newTask.Description
            };

            taskService.CreateTask(mapper.Map<TaskViewModel, TaskDTO>(task), author, newTask.Assignee, newTask.PriorityId, newTask.Deadline);

            return Ok(new { result = "Task has created!" });
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = taskService.GetTask(id);
            if (task == null)
            {
                ModelState.AddModelError("id", "Task hasn't found");
                return BadRequest(ModelState);
            }
            var resultTask = new TaskViewModel
            {
                Assignee = task.Assignee,
                Author = task.Author.FName + " " + task.Author.LName,
                Deadline = task.Deadline,
                Description = task.Description,
                FinishDate = task.FinishDate,
                Id = task.Id,
                Name = task.Name,
               // Priority = task.PriorityId,
                Progress = task.Progress,
                StartDate = task.StartDate,
                Status = task.Status.Name
            };

            return Ok(resultTask);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody]TaskUpdateViewModel taskUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string author = User.Identity.Name;
            var task = new TaskDTO
            {
                Id = id,
                Name = taskUpdate.Name,
                Description = taskUpdate.Description,
                Deadline = taskUpdate.Deadline,
               // PriorityId = taskUpdate.Priority,
                Assignee = taskUpdate.Assignee ?? author
            };

            taskService.UpdateTask(task, author);

            return Ok(new { message = "Task has changed" });
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("teamTasks/{id}")]
        public IActionResult TaskOfMyTeam(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonDTO manager = personService.GetPerson(id);

            var tasksOfMyTeam = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetTasksOfTeam(id));
            if (tasksOfMyTeam == null)
            {
                ModelState.AddModelError("", "Cannot find the tasks of current manager team!");
                return BadRequest(ModelState);
            }

            return Ok(tasksOfMyTeam);
        }

        [HttpGet("assignees/{id}")]
        public IActionResult GetAssignees(int managerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<PersonViewModel> assignees = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetAssignees(managerId));

            return Ok(assignees);
        }
    }
}