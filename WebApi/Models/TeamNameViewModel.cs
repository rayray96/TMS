using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class TeamNameViewModel
    {
        [Required(ErrorMessage = "Not specified team name")]
        public string TeamName { get; set; }
    }
}
