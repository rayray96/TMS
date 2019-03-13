namespace WebApi.Models
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public TeamViewModel Team { get; set; }
    }
}
