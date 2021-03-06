﻿using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            var managers = await userService.GetUsersInRoleAsync("Manager");
            var workers = await userService.GetUsersInRoleAsync("Worker");

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
            var user = await userService.GetUserByIdAsync(id);
            if (user != null)
            {
                Log.Information($"User with UserId: {id} was been found");
                return Ok(user);
            }
            else
            {
                ModelState.AddModelError("id", "User has not found");
                Log.Warning($"User with UserId: {id} was not found");
                return BadRequest(ModelState);
            }
        }

        // PUT api/admin/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(string Id, [FromBody]RoleChangeViewModel model)
        {
            if (Id == null || model.Role == null)
            {
                ModelState.AddModelError("", "UserId or roleName is null");
                Log.Warning("UserId or roleName is null");
                return BadRequest(ModelState);
            }
            var result = await userService.UpdateUserRoleAsync(Id, model.Role);
            if (result.Succeeded)
            {
                Log.Information($"Role for UserId: {Id} was been changed onto {model.Role}");
                return Ok();
            }
            else
            {
                ModelState.AddModelError(result.Property, result.Message);
                Log.Error("Updating a role was been failed");
                return BadRequest(ModelState);
            }
        }
    }
}