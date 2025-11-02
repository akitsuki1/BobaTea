using BobaTea.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BobaTea.Models; // đổi theo namespace thật của bạn

namespace BobaTea.Controllers
{
    public class AccountController : Controller
    {
        // Danh sách người dùng tạm (chưa kết nối DB)
        private static List<User> users = new List<User>();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                users.Add(model);
                TempData["Message"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);
            if (user != null)
            {
                Session["User"] = user.Username;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu sai.");
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
