using ChatAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChatAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
           _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {UserMasterViewModel model = new UserMasterViewModel();

            if (_httpContextAccessor.HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // Replace 'Students' with your actual DbSet for students
            model.UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            return View(model);
        }
        public IActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            model.IsSuccess = false;
            base.TempData["Message"] = "Login failed";
            if (model.UserName != null && model.UserPassword != null)
            {
                if (model.UserName.Trim() == "user1" && model.UserPassword.Trim() == "Admin123")
                {

                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", 1);
                    model.IsSuccess = true;
                    base.TempData["Message"] = null;
                }
               else if (model.UserName.Trim() == "user2" && model.UserPassword.Trim() == "Admin123")
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", 2);
                    model.IsSuccess = true;
                    base.TempData["Message"] = null;
                }
            }
            return Json(model);
        }
        public IActionResult Logout()
        {
            try
            {

                _httpContextAccessor.HttpContext.Session.Remove("UserId");
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
