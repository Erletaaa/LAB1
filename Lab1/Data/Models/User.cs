namespace GarageMarketProject.Data.Models
{
    public class User : Base
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime Birthday { get; set; }
        public string ProfilePicureUrl { get; set; }
        public string ProfileDescription { get; set; }
    }
}
