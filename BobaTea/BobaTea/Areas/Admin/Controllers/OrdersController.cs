using System.Linq;
using System.Web.Mvc;
using BobaTea.Models;

namespace BobaTea.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private BobaTeaEntities db = new BobaTeaEntities();

        public ActionResult Index()
        {
            var orders = db.OrderPro
                           .OrderByDescending(o => o.DateOrder)
                           .ToList();

            return View(orders);
        }
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();

            var order = db.OrderPro.Find(id);
            if (order == null) return HttpNotFound();

            var details = db.OrderDetail.Where(x => x.IDOrder == id).ToList();
            ViewBag.Order = order;

            return View(details);
        }
    }
}
