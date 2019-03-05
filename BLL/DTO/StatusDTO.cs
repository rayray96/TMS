using System.Collections.Generic;

namespace BLL.DTO
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TaskDTO> Tasks { get; set; }
    }
}
