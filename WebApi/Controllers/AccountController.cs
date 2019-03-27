using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AccountModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;

        public AccountController(IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }
        // POST api/account/sign-up
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUpAsync([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Password = model.Password,
                    FName = model.FName,
                    LName = model.LName
                };
                var result = await userService.CreateUserAsync(user);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        // POST api/account/sign-in
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult> SignInAsync([FromBody]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await tokenService.GetClaimsIdentityAsync(login.UserName, login.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login", "Invalid username or password");
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 120);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(login.UserName);
            var role = await userService.GetUserAsync(login.UserName);

            var response = new
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                Login = login.UserName,
                Role = role.Role
            };

            return Ok(response);
        }
        // POST api/account/{refreshToken}/refresh
        [HttpPost("{refreshToken}/refresh")]
        public async Task<ActionResult> GetRefreshTokenAsync([FromRoute]string refreshToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _refreshToken = tokenService.GetRefreshToken(refreshToken);
            if (_refreshToken == null)
                return BadRequest();

            if (_refreshToken.Expires < DateTime.Now)
                return Unauthorized();

            var identity = await tokenService.GetClaimsIdentityAsync(_refreshToken.UserId);
            if (identity == null)
            {
                ModelState.AddModelError("userId", "Invalid userId");
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 10);
            tokenService.UpdateRefreshToken(_refreshToken);

            var response = new
            {
                AccessToken = token,
                RefreshToken = _refreshToken.Token,
                Login = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Role)
                       .Select(c => c.Value)
                       .FirstOrDefault(),
                Role = identity.Claims
                       .Where(c => c.Type == ClaimTypes.Role)
                       .Select(c => c.Value)
                       .FirstOrDefault()
            };

            return Ok(response);
        }
    }
}
