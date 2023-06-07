using Lab1.Data.Models;
using Lab1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Lab1.Data.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext _context;
        private int PageSize = 12;

        public UserHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            var user = _context.Users
                               .Include(x => x.Roles)
                                .ThenInclude(x => x.Role)
                               .Where(x => x.Id == id)
                               .AsNoTracking()
                               .FirstOrDefault();

            return user;
        }

        public User GetByEmail(string email)
        {
            var user = _context.Users
                                .Where(x => x.Email == email)
                                    .Include(x=>x.Roles)
                                .AsNoTracking()
                                .FirstOrDefault();
            return user;
        }

        public PagedResponse<List<User>> GetPagedUsers(int page)
        {
            var usersCount = _context.Users.OrderBy(x => x.Id).Count();
            var users = _context.Users.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            return new PagedResponse<List<User>>(users, page, usersCount);
        }

        public List<Role> GetRoles()
        {
            var roles = _context.Roles.AsNoTracking().ToList();
            return roles;
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

            user.Username = updateUserModel.Username;
            user.Email = updateUserModel.Email;
            user.HashedPassword = updateUserModel.HashedPassword;
            user.ProfilePicureUrl = updateUserModel.ProfilePicureUrl;
            user.ProfileDescription = updateUserModel.ProfileDescription;
            user.UpdatedOn = updateUserModel.UpdatedOn;

            _context.SaveChanges();
        }

        public void UpdateRoles(int userId, List<int> roleIds)
        {
            var roles = _context.UserRoles.Where(x => x.UserId == userId).ToList();

            _context.UserRoles.RemoveRange(roles);
            _context.UserRoles.AddRange(roleIds.Select(x => new UserRole
            {
                UserId = userId,
                RoleId = x
            }));
            _context.SaveChanges();
        }

        //todo: delete everything connected to user first
        public void DeleteUser(int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
                return;

            var products = _context.Products.Where(x => x.UserId == id);
            _context.Products.RemoveRange(products);

            var userRoles = _context.UserRoles.Where(x => x.UserId == id);
            _context.UserRoles.RemoveRange(userRoles);

            var follows = _context.Follows.Where(x => x.FromUserId == id || x.ToUserId == id);
            _context.Follows.RemoveRange(follows);

            var comments = _context.Comments.Where(x => x.UserId == id);
            _context.Comments.RemoveRange(comments);

            var favorites = _context.Favorites.Where(x => x.UserId == id);
            _context.Favorites.RemoveRange(favorites);

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
