using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [Route("getlogin")]
        //[HttpGet]
        public IActionResult GetLogin()
        {
            return Ok($"Your login: {User.Claims.First(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value}");
        }

        // GET api/values/5
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [Route("getrole")]
        //[HttpGet]
        public IActionResult GetRole()
        {
            return Ok("Your role: Admin");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
