using System.ComponentModel.DataAnnotations;

namespace WebApi.AccountModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Not specified username")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Not specified password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
