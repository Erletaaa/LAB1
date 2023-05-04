using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Models.ViewModels;
using Lab1.Data.Helpers;
using Lab1.Data;
using Lab1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryHelper _categoryHelper;
        private ProductHelper _productHelper;

        public CategoryController(ApplicationDbContext context)
        {
            _categoryHelper = new CategoryHelper(context);
            _productHelper = new ProductHelper(context);
        }

        [HttpGet()]
        public ActionResult GetAll()
        {
            var categories = _categoryHelper.GetAll();
            var formattedCategories = categories.Select(x => new CategoryViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            });

            return Ok(formattedCategories);
        }

        [HttpGet("browse")]
        public ActionResult GetProductsPaged(int categoryId, int page)
        {
            var category = _categoryHelper.GetById(categoryId);
            var (totalPages, products) = _productHelper.GetProductsForCategory(categoryId, page);
            var formattedModel = new CategoryPageViewModel
            {
                Category = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                },
                Products = products.Select(x => new ProductCardModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Picture = x.PictureUrl,
                    Price = x.Price
                }).ToList(),
                TotalPages = totalPages
            };

            return View(formattedModel);
        }
    }
}
