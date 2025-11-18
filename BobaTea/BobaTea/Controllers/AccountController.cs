using BobaTea.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BobaTea.Controllers
{
    public class AccountController : Controller
    {
        private BobaTeaEntities db = new BobaTeaEntities();

        // --- Đăng ký ---
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users model, string ConfirmPassword)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "❌ Dữ liệu nhập không hợp lệ.");
                return View(model);
            }

            // Kiểm tra tên đăng nhập tồn tại
            if (db.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "⚠️ Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                return View(model);
            }

            // Kiểm tra mật khẩu nhập lại
            if (model.PasswordHash != ConfirmPassword)
            {
                ModelState.AddModelError("", "⚠️ Mật khẩu xác nhận không khớp.");
                return View(model);
            }

            // Gán role mặc định
            model.Role = "User";
            model.CreatedAt = DateTime.Now;

            // Thêm vào DB
            db.Users.Add(model);
            db.SaveChanges();

            TempData["Message"] = "🎉 Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }


        // --- Đăng nhập ---
        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == Username && u.PasswordHash == Password);

            if (user != null)
            {
                // Lưu session
                Session["User"] = user;
                Session["Username"] = user.Username;
                Session["UserId"] = user.UserId;

                // Điều hướng theo quyền
                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "❌ Tên đăng nhập hoặc mật khẩu không đúng.");
            return View();
        }

        // --- Đăng xuất ---
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // --- Trang hồ sơ ---
        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["User"] == null)
            {
                TempData["Message"] = "⚠️ Vui lòng đăng nhập để xem thông tin cá nhân.";
                return RedirectToAction("Login");
            }

            var sessionUser = Session["User"] as Users;
            if (sessionUser == null)
            {
                TempData["Message"] = "❌ Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Login");
            }

            var user = db.Users.Find(sessionUser.UserId);

            if (user == null)
            {
                TempData["Message"] = "❌ Không tìm thấy tài khoản.";
                return RedirectToAction("Login");
            }

            return View(user); // <- View này phải có tên là "Profile.cshtml"
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(Users model)
        {
            if (Session["User"] == null)
                return RedirectToAction("Login");

            var sessionUser = Session["User"] as Users;
            if (sessionUser == null || sessionUser.UserId != model.UserId)
            {
                // Kiểm tra nếu UserId trong session và model trùng khớp
                return RedirectToAction("Login");
            }

            var user = db.Users.Find(model.UserId);
            if (user != null)
            {
                user.Email = model.Email;
                user.Phone = model.Phone;
                user.Address = model.Address;
                db.SaveChanges();

                TempData["Message"] = "✅ Cập nhật thành công!";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("", "❌ Cập nhật thất bại.");
            return View(model);
        }

    }
}
