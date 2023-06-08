using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Models;
using Lab1.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class AdminController : Controller
    {
        private UserHelper userHelper;
        private ProductHelper productHelper;
        private CategoryHelper categoryHelper;


        public AdminController(ApplicationDbContext context)
        {
            userHelper = new UserHelper(context);
            productHelper = new ProductHelper(context);
            categoryHelper = new CategoryHelper(context);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("Admin/Users")]
        public ActionResult GetUsers(int page = 1)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var pagedResponse = userHelper.GetPagedUsers(page);

            return View("AdminUsers", pagedResponse);
        }

        [HttpGet("Admin/Products")]
        public ActionResult GetProducts(int page = 1)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var pagedProducts = productHelper.GetPagedProducts(page);

            if (!pagedProducts.Item.Any())
                return View("AdminProducts", new PagedResponse<List<AdminProductModel>>(new List<AdminProductModel>(), page, 0));

            var viewProducts = pagedProducts.Item.Select(x => new AdminProductModel
            {
                Id = x.Id,
                Title = x.Title,
                Category = x.Category.Name,
                User = x.User.Email,
                UpdatedOn = x.UpdatedOn,
                Activity = x.ActivityStatus
            }).ToList();

            return View("AdminProducts", new PagedResponse<List<AdminProductModel>>(viewProducts,page, pagedProducts.TotalCount));
        }

        [HttpGet("Admin/Edit/User")]
        public ActionResult GetUser(int id)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var user = userHelper.GetById(id);
            var roles = userHelper.GetRoles();

            return View("EditUser", new AdminUserModel
            {
                Id=user.Id,
                Username=user.Username,
                Email = user.Email,
                HashedPassword = user.HashedPassword,
                Roles = user.Roles,
                AvailableRoles = roles
            });
        }

        [HttpPost("Admin/Edit/User")]
        public ActionResult EditUser(EditUserModel model)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var userById = userHelper.GetById(model.Id);

            if (userById == null)
                return RedirectToAction("GetUsers");

            var password = model.Password;
            if (userById.HashedPassword != password)
                password = userHelper.ComputeSha256Hash(model.Password);

            //Email can't be updated
            var modelToUpdate = new User
            {
                Id = model.Id,
                Username = model.Username,
                Email = userById.Email,
                HashedPassword = password,
                Birthday = userById.Birthday,
                ProfilePicureUrl = userById.ProfilePicureUrl,
                ProfileDescription = userById.ProfileDescription
            };

            userHelper.UpdateUser(model.Id, modelToUpdate);
            userHelper.UpdateRoles(model.Id, model.RoleIds);

            return RedirectToAction("GetUsers");
        }

        [HttpGet("Admin/Edit/Product")]
        public ActionResult GetProduct(int id)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var product = productHelper.GetProductDetailsById(id);
            var categories = categoryHelper.GetAll();

            return View("EditProduct", new ProductCategoryModel
            {
                Product = product,
                AvailableCategories = categories
            });
        }

        [HttpPost("Admin/Edit/Product")]
        public ActionResult EditProduct(EditProductModel model)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var productById = productHelper.GetProductById(model.Id);

            if (productById == null)
                return RedirectToAction("GetProducts");

            var productToUpdate = new Product
            {
                Title = model.Title,
                Description = model.Description,
                Price = productById.Price,
                CategoryId = model.CategoryId,
                UserPhoneNumber = model.UserPhoneNumber,
                UserAddress = model.UserAddress,
                PictureUrl = productById.PictureUrl,
                ActivityStatus = model.Status
            };

            productHelper.UpdateProduct(model.Id, productToUpdate);
            return RedirectToAction("GetProducts");
        }

        public ActionResult DeleteUser(int id)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var user = userHelper.GetById(id);

            if (user == null)
                return RedirectToAction("GetUsers");

            userHelper.DeleteUser(id);

            return RedirectToAction("GetUsers");
        }

        public ActionResult DeleteProduct(int id)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            if (!AuthorizeAdmin(userSessionValue))
                return RedirectToAction("Index", "Home");

            var product = productHelper.GetProductById(id);

            if (product == null)
                return RedirectToAction("GetProducts");

            productHelper.DeleteProduct(id);

            return RedirectToAction("GetProducts");
        }

        private bool AuthorizeAdmin(string userSession)
        {
            var user = JsonConvert.DeserializeObject<User>(userSession);

            //2 is the id of the admin role
            if (user.Roles.Any(x => x.RoleId == 2))
                return true;

            return false;
        }
    }
}
