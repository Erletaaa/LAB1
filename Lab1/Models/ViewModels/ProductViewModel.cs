using Lab1.Models.ViewModels;

namespace Lab1.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public string Picture { get; set; }
        public CategoryViewModel Category { get; set; }
        public UserViewModel User { get; set; }
        public int Favorites { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
