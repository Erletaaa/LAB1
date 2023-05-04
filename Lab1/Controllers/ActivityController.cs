using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class ActivityController : Controller
    {
        private ActivityHelper _activityHelper;

        public ActivityController(ApplicationDbContext context)
        {
            _activityHelper = new ActivityHelper(context);
        }

        [HttpPost("Activity/Favorite/{productId}")]
        public ActionResult Favorite(int productId)
        {
            var user = HttpContext.Session.GetString("User");

            if (user == null)
                return BadRequest();

            var userModel = JsonConvert.DeserializeObject<User>(user);
            var favorite = _activityHelper.AddFavorite(productId, userModel.Id);

            if (favorite == null)
                return BadRequest();

            return Ok();
        }

        [HttpPost("Activity/Follow/{userId}")]
        public ActionResult Follow(int userId)
        {
            var user = HttpContext.Session.GetString("User");

            if (user == null)
                return BadRequest();

            var userModel = JsonConvert.DeserializeObject<User>(user);
            var follow = _activityHelper.AddFollow(userModel.Id, userId);

            if (follow == null)
                return BadRequest();

            return Ok();
        }
    }
}
