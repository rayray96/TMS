using System.Collections.Generic;

namespace DAL.Entities
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TaskInfo> Tasks { get; set; }

        public Priority()
        {
            Tasks = new List<TaskInfo>();
        }
    }
}
