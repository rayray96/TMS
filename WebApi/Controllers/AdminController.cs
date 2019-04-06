using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.AccountModels;
using WebApi.Configurations;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
            mapper = MapperConfig.GetMapperResult();
        }
        // GET api/admin
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managers = await userService.GetAllManagersAsync();
            var workers = await userService.GetAllWorkersAsync();

            var users = new List<UserDTO>();
            users.AddRange(managers);
            users.AddRange(workers);

            var model = mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(users);

            return Ok(model);
        }
        // GET api/admin/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userService.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("id", "User has not found");
                return BadRequest(ModelState);
            }
        }
        // PUT api/admin/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(string Id, [FromBody]RoleChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id == null || model.Role == null)
            {
                ModelState.AddModelError("", "Invalid userId or roleName");
                return BadRequest(ModelState);
            }
            var result = await userService.UpdateUserRoleAsync(Id, model.Role);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(result.Property, result.Message);
                return BadRequest(ModelState);
            }
        }
    }
}