using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using WebApi.Configurations;
using WebApi.Filters;
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

        // GET api/team/{id}
        [UserValidationActionFilter]
        [HttpGet("{id}")]
        public IActionResult GetMyTeam(string id)
        {
            var person = personService.GetPerson(id);
            if (person.TeamId == null)
            {
                ModelState.AddModelError("team", "Current person does not have a team");
                Log.Warning($"User with UserId: {id} does not have a team");
                return BadRequest(ModelState);
            }
            string teamName = teamService.GetTeamNameById(person.TeamId.Value);

            var teamOfCurrentManager = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetTeam(id));

            var teamModel = new TeamViewModel
            {
                TeamName = (teamName != null) ? teamName : string.Empty,
                Team = teamOfCurrentManager.ToList()
            };

            Log.Information($"Team with Id: {person.TeamId.Value} was been found for the user with UserId: {id}");
            return Ok(teamModel);
        }

        // GET api/team/possibleMembers
        [HttpGet("possibleMembers")]
        public IActionResult GetPossibleMembers()
        {
            var persons = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetPeopleWithoutTeam());

            return Ok(persons);
        }

        // POST api/team/{id}
        [UserValidationActionFilter]
        [HttpPost("{id}")]
        public IActionResult CreateTeam(string id, [FromBody]TeamNameViewModel model)
        {
            var author = personService.GetPerson(id);
            teamService.CreateTeam(author, model.TeamName);

            Log.Information($"Team was been created for the user with the UserId: {id}");
            return Ok(new { message = "The team has created!" });
        }

        // POST api/team/addMembers/{id}
        [UserValidationActionFilter]
        [HttpPost("addMembers/{Id}")]
        public IActionResult AddMembersToTeam(string Id, [FromBody]AddMembersViewModel members)
        {
            personService.AddPersonsToTeam(members.Members, Id);

            Log.Information($"Members were been added to the team of the user with UserId: {Id}");
            return Ok(new { message = "Members have added to your team" });
        }

        // PUT api/team/{id}
        [UserValidationActionFilter]
        [HttpPut("{id}")]
        public IActionResult UpdateTeamName(string Id, [FromBody]TeamNameViewModel model)
        {
            var author = personService.GetPerson(Id);
            if (author.TeamId == null)
            {
                ModelState.AddModelError("team", "Current author does not have a team");
                Log.Warning($"Author with UserId: {Id} does not have a team");
                return BadRequest(ModelState);
            }
            teamService.ChangeTeamName(author.TeamId.Value, model.TeamName);

            Log.Information($"Team name was been changed by the user with UserId: {Id}");
            return Ok(new { message = "The team name has been successfully changed" });
        }

        // DELETE api/team/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteFromTeam(int id)
        {
            personService.DeletePersonFromTeam(id);

            Log.Information($"Team member with PersonId: {id} was been deleted from the team");
            return Ok(new { message = "Member has deleted from your team" });
        }
    }
}
