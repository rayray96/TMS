using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class EditStatusViewModel
    {
        [Required(ErrorMessage = "Not specified task")]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Not specified status")]
        public string Status { get; set; }
    }
}
