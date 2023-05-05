using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    public class UserController : Controller
    {
        private UserHelper _userHelper;
        private ProductHelper _productHelper;

        public UserController(ApplicationDbContext context)
        {
            _userHelper = new UserHelper(context);
            _productHelper = new ProductHelper(context);
        }

        [HttpGet]
        public ActionResult Profile(int id)
        {
            var user = _userHelper.GetById(id);
            var usersProducts = _productHelper.GetProductsForUser(id);

            return View(new UserProfileViewModel
            {
                User = user,
                Products = usersProducts.Select(x => new ProductCardModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Picture = x.PictureUrl,
                    Price = x.Price
                }).ToList()
            });
        }
    }
}
