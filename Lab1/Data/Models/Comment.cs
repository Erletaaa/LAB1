namespace Lab1.Data.Models
{
    public class Comment : Base
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string CommentText { get; set; }
    }
}
