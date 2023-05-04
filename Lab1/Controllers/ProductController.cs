using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Models;
using Lab1.Models.ViewModels;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Data;
using Lab1.Models.ViewModels;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class ProductController : Controller
    {
        private ProductHelper _productHelper;
        private CategoryHelper _categoryHelper;
        private ActivityHelper _activityHelper;
        private UserHelper _userHelper;

        private FileHelper _fileHelper;

        public ProductController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _productHelper = new ProductHelper(context);
            _categoryHelper = new CategoryHelper(context);
            _activityHelper = new ActivityHelper(context);
            _userHelper = new UserHelper(context);

            _fileHelper = new FileHelper(hostingEnvironment);
        }

        [HttpGet("/create")]
        public ActionResult Create()
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");


            ViewBag.CategoryOptions = _categoryHelper.GetAll();
            return View();
        }

        [HttpPost("/create")]
        public ActionResult Create(ProductModel productModel)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            var user = JsonConvert.DeserializeObject<User>(userSessionValue);
            var uploadedFile = _fileHelper.DownloadFile(PictureGroupEnum.PRODUCT, productModel.Picture);

            var product = new Product
            {
                Title = productModel.Title,
                Description = productModel.Description,
                CategoryId = productModel.CategoryId,
                UserId = user.Id,
                UserPhoneNumber = productModel.UserPhoneNumber,
                UserAddress = productModel.UserAddress,
                PictureUrl = uploadedFile,
                ActivityStatus = ActivityEnum.ACTIVE
            };

            var insertedProduct = _productHelper.AddProduct(product);

            return RedirectToAction("Product", new { Id = insertedProduct.Id });
        }

        [HttpGet("product/{id}")]
        public ActionResult Product(int id)
        {
            var product = _productHelper.GetProductById(id);

            if (product == null)
                return RedirectToAction("Index", "Home");

            var category = _categoryHelper.GetById(product.CategoryId);
            var user = _userHelper.GetById(product.UserId);
            var favoriteCount = _activityHelper.FavoritesCount(id);

            var productModel = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Picture = product.PictureUrl,
                UserPhoneNumber = product.UserPhoneNumber,
                UserAddress = product.UserAddress,
                Category = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                },
                User = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    ProfilePicture = user.ProfilePicureUrl
                },
                Favorites = (int)favoriteCount
            };

            return View(productModel);
        }
    }
}
