using System.Collections.Generic;

namespace DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public Team()
        {
            People = new List<Person>();
        }
    }
}
