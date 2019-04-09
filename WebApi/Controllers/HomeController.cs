using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Configurations;
using WebApi.AccountModels;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITaskService taskService;
        private readonly IMapper mapper;

        public HomeController(IUserService userService, ITaskService taskService)
        {
            this.userService = userService;
            this.taskService = taskService;
            mapper = MapperConfig.GetMapperResult();
        }
        // GET api/home/userProfile
        [HttpGet("userProfile")]
        public async Task<ActionResult> GetUserProfile()
        {
            string userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await userService.GetUserByNameAsync(userName);

            var model = mapper.Map<UserDTO, UserViewModel>(user);

            return Ok(model);
        }
        // GET api/home/tasks
        [HttpGet("tasks")]
        public ActionResult GetAllTasks()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myTasks = mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetAllTasks());

            return Ok(myTasks);
        }
    }
}
