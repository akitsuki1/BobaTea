using System.Web.Mvc;

namespace BobaTea.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ (Intro)
        public ActionResult Index()
        {
            return View(); // Hiển thị file Views/Home/Intro.cshtml
        }

        // Trang thông tin
        public ActionResult Information()
        {
            return View();
        }

        // Trang sản phẩm
        public ActionResult Produce()
        {
            return View();
        }

        // Trang tuyển dụng
        public ActionResult Apply()
        {
            return View();
        }

        // Trang giỏ hàng
        public ActionResult Cart()
        {
            return View();
        }

        // Trang đăng nhập
        public ActionResult Login()
        {
            return View();
        }
    }
}
