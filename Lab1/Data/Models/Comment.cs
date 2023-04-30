namespace GarageMarketProject.Data.Models
{
    public class Comment : Base
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string CommentText { get; set; }
    }
}
