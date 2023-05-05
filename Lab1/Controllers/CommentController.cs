using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Models;
using Lab1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class CommentController : Controller
    {
        private CommentHelper _commentHelper;
        public CommentController(ApplicationDbContext context)
        {
            _commentHelper = new CommentHelper(context);
        }

        [HttpPost("comment/add")]
        public ActionResult AddComment(CommentModel commentModel)
        {
            if (string.IsNullOrEmpty(commentModel.CommentText))
            {
                return BadRequest();
            }

            var userSessionValue = HttpContext.Session.GetString("User");

            if (userSessionValue == null)
                return BadRequest();

            var user = JsonConvert.DeserializeObject<User>(userSessionValue);

            var comment = _commentHelper.AddComment(new Comment
            {
                ProductId = commentModel.ProductId,
                UserId = user.Id,
                CommentText = commentModel.CommentText
            });

            if (comment == null)
                return BadRequest();

            var formattedComment = new CommentViewModel
            {
                Id = comment.Id,
                CommentText = comment.CommentText,
                Username = comment.User.Username,
                UpdatedOn = comment.UpdatedOn
            };

            return Ok(formattedComment);
        }

        [HttpPost("comment/delete/{commentId}")]
        public ActionResult DeleteComment(int commentId)
        {
            var comment = _commentHelper.GetById(commentId);

            if (comment == null)
            {
                return BadRequest();
            }

            var userSessionString = HttpContext.Session.GetString("User");

            if (userSessionString == null)
                return BadRequest(); //not logged in

            var user = JsonConvert.DeserializeObject<User>(userSessionString);

            if (comment.UserId != user.Id)
            {
                return BadRequest(); //the authors and the person trying to delete are not the same
            }

            _commentHelper.DeleteComment(commentId);
            return Ok();
        }

        [HttpPost("comment/update")]
        public ActionResult UpdateComment(CommentModel commentModel)
        {
            var userSessionString = HttpContext.Session.GetString("User");

            if (userSessionString == null)
                return BadRequest(); //not logged in

            var user = JsonConvert.DeserializeObject<User>(userSessionString);

            var comment = _commentHelper.GetById(commentModel.Id);

            if (comment == null)
                return BadRequest();

            if (comment.UserId != user.Id)
                return BadRequest();

            var updatedComment = _commentHelper.UpdateComment(commentModel.Id, commentModel.CommentText);

            if (updatedComment == null)
                return BadRequest();

            return Ok();
        }
    }
}
