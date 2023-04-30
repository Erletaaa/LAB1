namespace GarageMarketProject.Data.Models
{
    public class Product : Base
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserAddress { get; set; }
        public string PictureUrl { get; set; }
        public ActivityEnum ActivityStatus { get; set; }
    }
}
