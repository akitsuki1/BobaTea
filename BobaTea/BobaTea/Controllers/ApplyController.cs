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
        public ActionResult SubmitForm(string name, string email, string message)
        {
            // Xử lý logic lưu form hoặc gửi email tại đây
            ViewBag.Success = "Cảm ơn bạn đã gửi thông tin!";
            return View("Index");
        }
    }
}
