using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Configurations;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamService;
        private readonly IPersonService personService;
        private readonly IMapper mapper;

        public TeamController(ITeamService teamService, IPersonService personService)
        {
            this.teamService = teamService;
            this.personService = personService;
            mapper = MapperConfig.GetMapperResult();
        }

        [HttpGet]
        public IActionResult GetMyTeam()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            PersonDTO person = personService.GetPerson(id);
            TeamDTO team = person.Team;

            IEnumerable<PersonViewModel> teamOfCurrentManager = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetTeam(id));

            return Ok(new
            {
                TeamName = (team != null) ? team.TeamName : string.Empty,
                Team = teamOfCurrentManager.ToList()
            });
        }

        [HttpPost("{id}")]
        public IActionResult CreateTeam(int id, [FromBody] string teamName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = personService.GetPerson(id);

            try
            {
                teamService.CreateTeam(author, teamName);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Team hasn't created.", e.Message);
                return BadRequest();
            }

            return Ok(new { result = "Team has created!" });
        }

        [HttpGet("possibleMembers")]
        public IActionResult GetPossibleMembers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<PersonViewModel> persons = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetPeopleWithoutTeam());

            return Ok(new
            {
                Name = "Possible Members",
                Value = persons.ToList()
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFromTeam(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                personService.DeletePersonFromTeam(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Members hasn't deleted", e.Message);
                return BadRequest();
            }
            return Ok("Members was deleted from your team");
        }

        [HttpPost("addMembers")]
        public IActionResult AddMembersToTeam(int[] persons)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                personService.AddPersonsToTeam(persons, managerId);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Members hasn't added", e.Message);
                return BadRequest();
            }

            return Ok("Members was added to your team");
        }
    }
}