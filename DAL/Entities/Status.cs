using System.Collections.Generic;

namespace DAL.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TaskInfo> Tasks { get; set; }

        public Status()
        {
            Tasks = new List<TaskInfo>();
        }
    }
}
