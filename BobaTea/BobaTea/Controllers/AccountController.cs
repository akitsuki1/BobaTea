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
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.PasswordHash))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View(model);
            }

            if (model.PasswordHash != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu không trùng khớp!";
                return View(model);
            }


            var checkUser = db.Users.FirstOrDefault(x => x.Username == model.Username);
            if (checkUser != null)
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View(model);
            }


            db.Users.Add(model);
            db.SaveChanges();

            Customer cus = new Customer
            {
                IDCus = model.UserId,
                NameCus = model.Username,
                PhoneCus = model.Phone, 
            };
            db.Customer.Add(cus);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Hãy đăng nhập.";
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
                // Lưu session User
                Session["User"] = user;
                Session["Username"] = user.Username;
                Session["UserId"] = user.UserId;

                // Lấy hoặc tạo Customer tương ứng
                var customer = db.Customer.FirstOrDefault(c => c.UserName == user.Username);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        NameCus = user.Username,
                        UserName = user.Username,
                        EmailCus = user.Email,
                        PhoneCus = user.Phone
                    };
                    db.Customer.Add(customer);
                    db.SaveChanges();
                }

                Session["CustomerId"] = customer.IDCus;

                // Điều hướng theo quyền
                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
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
                TempData["Message"] = "⚠ Vui lòng đăng nhập để xem thông tin cá nhân.";
                return RedirectToAction("Login");
            }

            var sessionUser = Session["User"] as Users;
            if (sessionUser == null)
            {
                TempData["Message"] = " Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Login");
            }

            var user = db.Users.Find(sessionUser.UserId);

            if (user == null)
            {
                TempData["Message"] = "❌ Không tìm thấy tài khoản.";
                return RedirectToAction("Login");
            }

            return View(user); // <-tới Profile.cshtml
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

                TempData["Message"] = " Cập nhật thành công!";
                return RedirectToAction("Profile");
            }

            ModelState.AddModelError("", " Cập nhật thất bại.");
            return View(model);
        }
        public bool CheckLogin()
        {
            if (Session["User"] == null)
            {
                TempData["LoginRequired"] = "⚠ Bạn cần đăng nhập để tiếp tục.";
                return false;
            }
            return true;
        }
    }
}
