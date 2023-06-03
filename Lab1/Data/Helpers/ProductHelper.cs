using Lab1.Data.Models;
using Lab1.Models;
using Lab1.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Data.Helpers
{
    public class ProductHelper
    {
        private readonly int PageSize = 12;

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

        public Product GetProductDetailsById(int id)
        {
            var product = _context.Products.Where(x => x.Id == id)
                                            .Include(x => x.Category)
                                            .FirstOrDefault();

            return product;
        }

        public List<Product> GetProductsForUser(int userId)
        {
            var products = _context.Products.Where(x => x.UserId == userId).ToList();
            return products;
        }

        public List<Product> GetLatestProducts(int limit)
        {
            var products = _context.Products.OrderByDescending(x => x.UpdatedOn).Take(limit).ToList();
            return products;
        }

        public (int,List<Product>) GetProductsForCategory(int categoryId, int page)
        {
            var products = _context.Products.Where(x => x.CategoryId == categoryId).Skip((page - 1) * PageSize).Take(PageSize).ToList();
            var productsCount = _context.Products.Where(x => x.CategoryId == categoryId).Count();
            var totalPages = (int)Math.Ceiling(productsCount / (double)PageSize);

            return (totalPages,products);
        }

        public PagedResponse<List<Product>> GetPagedProducts(int page)
        {
            var productCount = _context.Products.OrderBy(x => x.Id).Count();
            var products = _context.Products
                                    .Include(x => x.Category)
                                    .Include(x => x.User)
                                    .OrderBy(x => x.Id)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize)
                                    .ToList();

            return new PagedResponse<List<Product>>(products, page, productCount);
        }

        public Product AddProduct(Product product)
        {
            var insertedProduct = _context.Products.Add(product);
            _context.SaveChanges();

            return insertedProduct.Entity;
        }

        public void UpdateProduct(int id,Product updatedProductModel)
        {
            var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product == null)
                return;

            product.Title = updatedProductModel.Title;
            product.Description = updatedProductModel.Description;
            product.Price = updatedProductModel.Price;
            product.CategoryId = updatedProductModel.CategoryId;
            product.UserPhoneNumber = updatedProductModel.UserPhoneNumber;
            product.UserAddress = updatedProductModel.UserAddress;
            product.PictureUrl = updatedProductModel.PictureUrl;
            product.ActivityStatus = updatedProductModel.ActivityStatus;

            _context.SaveChanges();
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
