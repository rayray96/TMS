using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService userService;

        public UserProfileController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        //GET : /api/UserProfile
        public async Task<object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await userService.GetUser(userId);
            return new
            {
                user.FName,
                user.LName,
                user.Role,
                user.Email,
                user.UserName
            };
        }
    }
}