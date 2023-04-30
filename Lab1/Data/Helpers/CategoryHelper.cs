using GarageMarketProject.Data.Models;

namespace GarageMarketProject.Data.Helpers
{
    public class CategoryHelper
    {
        private ApplicationDbContext _context;
        public CategoryHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetById(int id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            return category;
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(int id, Category updateCategoryModel)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (category == null)
                return;

            category = updateCategoryModel;
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        //todo: delete everything connected to category
        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();

            if (category == null)
                return;

            _context.Categories.Remove(category);
        }
    }
}
