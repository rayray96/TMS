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

        [HttpGet("possibleMembers")]
        public IActionResult GetPossibleMembers()
        {
            IEnumerable<PersonViewModel> persons = mapper.Map<IEnumerable<PersonDTO>, IEnumerable<PersonViewModel>>(personService.GetPeopleWithoutTeam());

            return Ok(new
            {
                Name = "PossibleMembers",
                Value = persons.ToList()
            });
        }

        [HttpPost("{id}")]
        public IActionResult DeleteFromTeam(int id)
        {
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