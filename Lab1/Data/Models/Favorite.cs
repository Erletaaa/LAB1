namespace GarageMarketProject.Data.Models
{
    public class Favorite : Base
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}