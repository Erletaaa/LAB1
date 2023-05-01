namespace Lab1.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
    }
}
