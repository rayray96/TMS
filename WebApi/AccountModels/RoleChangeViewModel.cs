using System.ComponentModel.DataAnnotations;

namespace WebApi.AccountModels
{
    public class RoleChangeViewModel
    {
        [Required]
        [Display(Name = "New role")]
        public string Role { get; set; }
    }
}
