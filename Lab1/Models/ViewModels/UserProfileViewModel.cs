using Lab1.Data.Models;

namespace Lab1.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public List<ProductCardModel> Products { get; set; }
    }
}
