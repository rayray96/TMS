using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using WebApi.AccountModels;

namespace WebApi.Controllers
{
    [AllowAnonymous]
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
        [HttpPost("sign-up")]
        public async Task<ActionResult> SignUpAsync([FromBody]RegisterViewModel model)
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
                Log.Information("Sign up was been succeeded");
                return Ok(result);
            }
            else
            {
                Log.Error("Sign up was been failed");
                return BadRequest(result);
            }
        }
        // POST api/account/sign-in
        [HttpPost("sign-in")]
        public async Task<ActionResult> SignInAsync([FromBody]LoginViewModel login)
        {
            var identity = await tokenService.GetClaimsIdentityAsync(login.UserName, login.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login", "Invalid username or password");
                Log.Error("Claims-based identity is null");
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 120);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(login.UserName);
            var user = await userService.GetUserByNameAsync(login.UserName);

            var response = new
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                UserId = user.Id,
                user.Role
            };

            Log.Information($"User with UserId: {user.Id} and Role: {user.Role} has signed in");
            return Ok(response);
        }
        // POST api/account/{refreshToken}/refresh
        [HttpPost("{refreshToken}/refresh")]
        public async Task<ActionResult> GetRefreshTokenAsync([FromRoute]string refreshToken)
        {
            var _refreshToken = tokenService.GetRefreshToken(refreshToken);
            if (_refreshToken == null)
                return BadRequest();

            if (_refreshToken.Expires < DateTime.Now)
                return Unauthorized();

            var identity = await tokenService.GetClaimsIdentityAsync(_refreshToken.UserId);
            if (identity == null)
            {
                ModelState.AddModelError("userId", "Invalid userId");
                Log.Error("Claims-based identity is null");
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 10);
            tokenService.UpdateRefreshToken(_refreshToken);
            var user = await userService.GetUserByIdAsync(_refreshToken.UserId);

            var response = new
            {
                AccessToken = token,
                RefreshToken = _refreshToken.Token,
                _refreshToken.UserId,
                user.Role
            };

            Log.Information($"User with UserId: {user.Id} and Role: {user.Role} has got access and refresh tokens");
            return Ok(response);
        }
    }
}
