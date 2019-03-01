using System.Collections.Generic;

namespace DAL.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
