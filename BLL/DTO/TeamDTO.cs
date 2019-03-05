using System.Collections.Generic;

namespace BLL.DTO
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string ManagerName { get; set; }
        public string TeamName { get; set; }
        public virtual ICollection<PersonDTO> People { get; set; }
    }
}
