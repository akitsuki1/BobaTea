using System.Web.Mvc;

namespace BobaTea.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // Trang chính của admin (Dashboard)
        public ActionResult Index()
        {
            return View();
        }
    }
}
