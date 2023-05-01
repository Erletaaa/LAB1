using Lab1.Data.Models;

namespace Lab1.Data.Helpers
{
    public class CommentHelper
    {
        private ApplicationDbContext _context;
        public CommentHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public Comment GetById(int id)
        {
            var comment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            return comment;
        }

        public List<Comment> GetCommentsForProduct(int productId)
        {
            var comments = _context.Comments.Where(x => x.ProductId == productId).OrderByDescending(x => x.UpdatedOn).ToList();
            return comments;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void UpdateComment(int id, string commentText)
        {
            var comment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            if (comment == null)
                return;

            comment.CommentText = commentText;
            comment.UpdatedOn = DateTime.Now;

            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            var comment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();

            if (comment == null)
                return;

            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
