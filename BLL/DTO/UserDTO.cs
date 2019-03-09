using Microsoft.AspNetCore.Identity;

namespace BLL.DTO
{
    public class UserDTO//: IdentityUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string TeamName { get; set; }
        public string Role { get; set; }
    }
}
