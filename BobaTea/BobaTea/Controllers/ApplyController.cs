using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BobaTea.Controllers
{
    public class ApplyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitForm(string name, string email, string phone, string position, string message)
        {
            try
            {
                // TODO: xử lý lưu dữ liệu, upload file nếu cần

                return Json(new { success = true, msg = "✅ Đăng kí thành công! Cảm ơn bạn đã gửi thông tin." });
            }
            catch (Exception)
            {
                return Json(new { success = false, msg = "❌ Có lỗi xảy ra khi đăng kí!" });
            }
        }

    }
}
