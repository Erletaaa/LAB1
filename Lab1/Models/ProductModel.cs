using Lab1.Data.Models;

namespace Lab1.Models
{
    public class ProductModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public IFormFile Picture { get; set; }
    }
}
