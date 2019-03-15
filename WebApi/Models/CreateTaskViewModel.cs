using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CreateTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }
        public string Assignee { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
