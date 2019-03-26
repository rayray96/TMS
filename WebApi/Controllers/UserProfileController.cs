using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Configurations;
using WebApi.AccountModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserProfileController(IUserService userService)
        {
            this.userService = userService;
            mapper = MapperConfig.GetMapperResult();
        }

        // GET api/userProfile
        [HttpGet]
        public async Task<object> GetUserProfile()
        {
            string userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await userService.GetUser(userName);

            var model = mapper.Map<UserDTO, UserViewModel>(user);

            return Ok(model);
        }
    }
}