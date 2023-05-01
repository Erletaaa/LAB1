using Lab1.Data.Models;

namespace Lab1.Data.Helpers
{
    public class ProductHelper
    {
        private ApplicationDbContext _context;
        public ProductHelper(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Product GetProductById(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
            return product;
        }

        public Product AddProduct(Product product)
        {
            var insertedProduct = _context.Products.Add(product);
            _context.SaveChanges();

            return insertedProduct.Entity;
        }

        public bool UpdateProduct(int id, Product updatedProductModel)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product == null)
                return false;

            product = updatedProductModel;
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }

        //todo: delete the favorites and everything that can be connected
        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product == null)
                return false;

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
