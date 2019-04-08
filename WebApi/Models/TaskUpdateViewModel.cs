using System;

namespace WebApi.Models
{
    public class TaskUpdateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Assignee { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
