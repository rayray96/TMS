using System;

namespace WebApi.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PriorityViewModel PriorityId { get; set; }
        public PersonViewModel Author { get; set; }
        public PersonViewModel Assignee { get; set; }
        public StatusViewModel Status { get; set; }
        public int? Progress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
