using Lab1.Data.Models;
using Lab1.Data.Models;

namespace Lab1.Models.Admin
{
    public class ProductCategoryModel
    {
        public Product Product { get; set; }
        public List<Category> AvailableCategories { get; set; }
    }
}
