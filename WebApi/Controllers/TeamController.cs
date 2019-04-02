using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{id}")]
        public IActionResult GetMyTeam(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PersonDTO person = personService.GetPerson(id);
            string teamName = teamService.GetTeamNameById(person.TeamId);

            var teamOfCurrentManager = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetTeam(id));

            var teamModel = new TeamViewModel
            {
                TeamName = (teamName != null) ? teamName : string.Empty,
                Team = teamOfCurrentManager.ToList()
            };

            return Ok(teamModel);
        }

        [HttpGet("possibleMembers")]
        public IActionResult GetPossibleMembers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persons = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetPeopleWithoutTeam());

            return Ok(persons);
        }

        [HttpPost("{id}")]
        public IActionResult CreateTeam(string id, [FromBody]TeamNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = personService.GetPerson(id);
            teamService.CreateTeam(author, model.TeamName);

            return Ok(new { message = "The team has created!" });
        }

        [HttpPost("addMembers/{Id}")]
        public IActionResult AddMembersToTeam(string Id, [FromBody]AddMembersViewModel members)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            personService.AddPersonsToTeam(members.Members, Id);

            return Ok(new { message = "Members have added to your team" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeamName(string Id, [FromBody]TeamNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = personService.GetPerson(Id);
            teamService.ChangeTeamName(author.TeamId, model.TeamName);

            return Ok(new { message = "The team name has been successfully changed" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFromTeam(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            personService.DeletePersonFromTeam(id);

            return Ok(new { message = "Member has deleted from your team" });
        }
    }
}