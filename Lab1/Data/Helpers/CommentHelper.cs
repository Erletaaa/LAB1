using Lab1.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            var comment = _context.Comments.Where(x => x.Id == id).Include(x=>x.User).FirstOrDefault();
            return comment;
        }

        public List<Comment> GetCommentsForProduct(int productId)
        {
            var comments = _context.Comments.Where(x => x.ProductId == productId).Include(x=>x.User).OrderByDescending(x => x.UpdatedOn).ToList();
            return comments;
        }

        public Comment AddComment(Comment comment)
        {
            var insertedComment = _context.Comments.Add(comment);
            _context.SaveChanges();

            return GetById(insertedComment.Entity.Id);
        }

        public Comment UpdateComment(int id, string commentText)
        {
            var comment = _context.Comments.Where(x => x.Id == id).FirstOrDefault();
            if (comment == null)
                return null;

            comment.CommentText = commentText;
            comment.UpdatedOn = DateTime.Now;

            var updatedComment = _context.Comments.Update(comment);
            _context.SaveChanges();

            return updatedComment.Entity;
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
