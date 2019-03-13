using System.ComponentModel.DataAnnotations;

namespace WebApi.AccountModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Not specified Email")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
