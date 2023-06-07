namespace Lab1.Models.Admin
{
    public class EditUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
