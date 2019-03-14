using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterViewModel model)
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
                if (result.Succeedeed)
                {
                    return Ok(model);
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ActionResult> AccessToken([FromBody]LoginViewModel login)
        {
            var identity = await tokenService.GetClaimsIdentityAsync(login.Name, login.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login", "Invalid username or password");
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 2);

            var refreshToken = await tokenService.GenerateRefreshTokenAsync(login.Name);

            var response = new
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                Logsn = login.Name
            };

            return Ok(response);
        }

        [HttpPost("{refreshToken}/refresh")]
        public async Task<ActionResult> RefreshToken([FromRoute]string refreshToken)
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
                return BadRequest(ModelState);
            }

            var token = tokenService.GenerateToken(identity.Claims, 3);
            tokenService.UpdateRefreshToken(_refreshToken);

            var response = new
            {
                AccessToken = token,
                RefreshToken = _refreshToken.Token,
                Login = identity.Claims.First()
            };

            return Ok(response);
        }
    }
}
