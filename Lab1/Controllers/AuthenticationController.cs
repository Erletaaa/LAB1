using Lab1.Data;
using Lab1.Data.Helpers;
using Lab1.Data.Models;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lab1.Controllers
{
    public class AuthenticationController : Controller
    {
        private UserHelper _userHelper;
        private FileHelper _fileHelper;

        public AuthenticationController(
            ApplicationDbContext context,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _userHelper = new UserHelper(context);
            _fileHelper = new FileHelper(hostingEnvironment);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            var user = _userHelper.GetByEmail(loginModel.Email);

            if (user == null)
                return View();

            var hashedPassword = _userHelper.ComputeSha256Hash(loginModel.Password);
            if (hashedPassword == user.HashedPassword)
            {
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (HttpContext.Session.TryGetValue("User", out byte[] user))
            {
                HttpContext.Session.Remove("User");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            var user = _userHelper.GetByEmail(registerModel.Email);

            //if a user is found an account with that email is already created
            if (user != null)
            {
                return View();
            }

            var userModel = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                ProfileDescription = registerModel.Description,
                HashedPassword = _userHelper.ComputeSha256Hash(registerModel.Password),
                Birthday = registerModel.Birthday,
                ProfilePicureUrl = _fileHelper.DownloadFile(PictureGroupEnum.USER, registerModel.Picture)
            };

            _userHelper.AddUser(userModel);
            return RedirectToAction("Login");
        }
    }
}
