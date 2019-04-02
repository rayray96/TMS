namespace WebApi.Models
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int? TeamId { get; set; }
        public TeamViewModel Team { get; set; }
    }
}
