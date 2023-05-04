namespace Lab1.Models.ViewModels
{
    public class CategoryPageViewModel
    {
        public CategoryViewModel Category { get; set; }
        public List<ProductCardModel> Products { get; set; }
        public int TotalPages { get; set; } 
    }
}
