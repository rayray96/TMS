using System;

namespace DAL.Entities
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? PriorityId { get; set; } // TODO: Delete Nullable!
        public int AuthorId { get; set; }
        public int AssigneeId { get; set; }
        public int StatusId { get; set; }
        public int? Progress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? Deadline { get; set; }

        public virtual Person Assignee { get; set; }
        public virtual Person Author { get; set; }
        public virtual Status Status { get; set; }
        public virtual Priority Priority { get; set; }
    }
}
