namespace GarageMarketProject.Data.Models
{
    public class Follow : Base
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
    }
}