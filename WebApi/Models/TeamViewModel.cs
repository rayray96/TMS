using System.Collections.Generic;

namespace WebApi.Models
{
    public class TeamViewModel
    {
        public string TeamName { get; set; }
        public ICollection<PersonViewModel> Team { get; set; }
    }
}
