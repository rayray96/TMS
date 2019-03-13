using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
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

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDTO
                {
                    Email = model.Email,
                    UserName = model.Name,
                    Password = model.Password
                };
                var result = await userService.CreateAsync(user);
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

        [HttpPost("token")]
        public async Task<ActionResult> Login([FromBody]LoginViewModel login)
        {

            var identity = await userService.GetClaimsIdentityAsync(login.Name, login.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login", "Invalid username or password");
                return BadRequest(ModelState);
                //Response.StatusCode = 400;
                //await Response.WriteAsync("Invalid username or password");
                //return;
            }
            var token = userService.GenerateToken(identity.Claims, 1);
            var refreshToken = Guid.NewGuid().ToString();
            //return Ok(new { token });
            var _refreshToken = await userService.SetRefreshToken(login.Name, refreshToken);
            //var _refreshTokenObj = new RefreshToken
            //{
            //    Username = login.Name,
            //    Refreshtoken = Guid.NewGuid().ToString()
            //};
            //_db.RefreshTokens.Add(_refreshTokenObj);
            //_db.SaveChanges(); // move this part if program to SetRefreshToken.

            return Ok(new
            {
                token,
                refreshToken// = _refreshTokenObj.Refreshtoken
            });

            //return BadRequest("Could not verify username and password");

            //var response = new
            //{
            //    access_token = encodedJwt,
            //    username = identity.Name
            //};

            //// Serialization response.
            //Response.ContentType = "application/json";
            //await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        [HttpPost("{refreshToken}/refresh")]
        public async Task<ActionResult> RefreshToken([FromRoute]string refreshToken)
        {
            var _refreshToken = userService.GetRefreshToken(refreshToken);//_db.RefreshTokens.SingleOrDefault(m => m.Refreshtoken == refreshToken);

            if (_refreshToken == null)
            {
                return NotFound("Refresh token not found");
            }
            //var userclaim = new[] { new Claim(ClaimTypes.Name, _refreshToken.Username) };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: "https://localhost:5001",
            //    audience: "https://localhost:5001",
            //    claims: userclaim,
            //    expires: DateTime.Now.AddMinutes(2),
            //    signingCredentials: creds);

            var identity = await userService.GetClaimsIdentityAsync(_refreshToken.UserId);
            var token = userService.GenerateToken(identity.Claims, 10);
            _refreshToken.Refreshtoken = Guid.NewGuid().ToString();
            //_db.RefreshTokens.Update(_refreshToken);
            //_db.SaveChanges();

            return Ok(new { token, refToken = _refreshToken.Refreshtoken });
        }
    }
}
