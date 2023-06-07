using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Models;
using Lab1.Models.Admin;
using Lab1.Models.ViewModels;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Data;
using Lab1.Models.Admin;
using Lab1.Models.ViewModels;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;

namespace Lab1.Controllers
{
    public class ProductController : Controller
    {
        private ProductHelper _productHelper;
        private CategoryHelper _categoryHelper;
        private ActivityHelper _activityHelper;
        private UserHelper _userHelper;
        private CommentHelper _commentHelper;

        private FileHelper _fileHelper;

        public ProductController(
            ApplicationDbContext context,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _productHelper = new ProductHelper(context);
            _categoryHelper = new CategoryHelper(context);
            _activityHelper = new ActivityHelper(context);
            _userHelper = new UserHelper(context);
            _commentHelper = new CommentHelper(context);

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
            var comments = _commentHelper.GetCommentsForProduct(product.Id);
            var relatedProducts = _productHelper.GetRelatedProducts(product.CategoryId);

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
                Favorites = favoriteCount,
                Comments = comments.Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    CommentText = x.CommentText,
                    Username = x.User.Username,
                    UpdatedOn = x.UpdatedOn
                }).ToList(),
                RelatedProducts = relatedProducts.Select(x => new ProductCardModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Picture = x.PictureUrl,
                    Price = x.Price
                }).ToList()
            };

            return View(productModel);
        }

        [HttpGet("Product/EditProduct")]
        public ActionResult EditProduct(int id)
        {
            var product = _productHelper.GetProductById(id);

            if (product == null)
                return RedirectToAction("GetUserProducts");

            var categories = _categoryHelper.GetAll();

            return View(new ProductCategoryModel
            {
                Product = product,
                AvailableCategories = categories
            });
        }

        public ActionResult EditProduct(ProductModel model)
        {
            var product = _productHelper.GetProductById(model.Id);

            if (product is null)
                return RedirectToAction("Profile", "User");

            var fileName = product.PictureUrl;
            if (model.Picture is not null)
            {
                fileName = _fileHelper.DownloadFile(PictureGroupEnum.PRODUCT, model.Picture);
            }

            var updateModel = new Product
            {
                Title = product.Title,
                Description = product.Description,
                CategoryId = product.CategoryId,
                UserAddress = product.UserAddress,
                UserPhoneNumber = product.UserPhoneNumber,
                PictureUrl = fileName
            };

            _productHelper.UpdateProduct(model.Id, updateModel);

            return RedirectToAction("Profile", "User");
        }

        [HttpGet("Product/LatestProducts")]
        public ActionResult GetUserProducts(int page = 1)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            var user = JsonConvert.DeserializeObject<User>(userSessionValue);
            var pagedProducts = _productHelper.GetUserPagedProducts(user.Id, page, 4);

            var processedProducts = pagedProducts.Item.Select(x => new ProductCardModel
            {
                Id = x.Id,
                Title = x.Title,
                Picture = x.PictureUrl,
                Price = x.Price
            }).ToList();

            return View("LatestProducts", new PagedResponse<List<ProductCardModel>>(processedProducts, pagedProducts.Page, pagedProducts.TotalCount, 4));
        }

        public ActionResult DeleteProduct(int id)
        {
            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return RedirectToAction("Login", "Authentication");

            var user = JsonConvert.DeserializeObject<User>(userSessionValue);

            var product = _productHelper.GetProductById(id);

            if (product.UserId != user.Id)
                return RedirectToAction("GetUserProducts");

            _productHelper.DeleteProduct(id);

            return RedirectToAction("GetUserProducts");
        }
    }
}
