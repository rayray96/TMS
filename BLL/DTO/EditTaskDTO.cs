using System;

namespace BLL.DTO
{
    public class EditTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Assignee { get; set; }
        public int AssigneeId { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
