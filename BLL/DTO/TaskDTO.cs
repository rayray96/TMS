using System;

namespace BLL.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }
        public PersonDTO Author { get; set; }
        public string Assignee { get; set; }
        public StatusDTO Status { get; set; }
        public int? Progress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
