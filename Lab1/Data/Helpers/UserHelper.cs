using Lab1.Data.Models;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Lab1.Data.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext _context;
        public UserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            return user;
        }

        public User GetByEmail(string email)
        {
            var user = _context.Users.Where(x => x.Email == email).FirstOrDefault();
            return user;
        }

        public void AddUser(User user)
        {
            var createdUser = _context.Users.Add(user);
            _context.SaveChanges();

            var userRole = new UserRole
            {
                UserId = createdUser.Entity.Id,
                RoleId = 1 //default role
            };

            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }

        public void UpdateUser(int id, User updateUserModel)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                return;

            user = updateUserModel;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        //todo: delete everything connected to user first
        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                return;

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
