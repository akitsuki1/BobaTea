using BobaTea.Models;
using System.Linq;
using System.Web.Mvc;

namespace BobaTea.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private BobaTeaEntities db = new BobaTeaEntities();

        // Danh sách người dùng
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // Chi tiết người dùng
        public ActionResult Details(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        // Chỉnh sửa người dùng
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserId);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.Role = model.Role;
                    db.SaveChanges();
                    TempData["Message"] = "✅ Cập nhật người dùng thành công!";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // Xóa người dùng
        [HttpPost] [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
                return Json(new { success = false, message = "Không tìm thấy người dùng" });

            db.Users.Remove(user);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}
