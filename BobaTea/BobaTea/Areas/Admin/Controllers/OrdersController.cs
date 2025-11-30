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

            // Lấy đơn hàng
            var order = db.OrderPro.FirstOrDefault(o => o.ID == id);
            if (order == null) return HttpNotFound();

            // Lấy thông tin khách hàng
            var customer = db.Customer.FirstOrDefault(c => c.IDCus == order.IDCus);

            // Lấy chi tiết đơn hàng
            var details = db.OrderDetail.Where(d => d.IDOrder == order.ID).ToList();

            ViewBag.Order = order;
            ViewBag.Customer = customer;

            return View(details); 
        }

        public ActionResult Delete(int id)
        {
            var order = db.OrderPro.Find(id);
            if (order == null)
                return HttpNotFound();

            // Xóa chi tiết đơn hàng
            var details = db.OrderDetail.Where(x => x.IDOrder == id).ToList();
            foreach (var d in details)
            {
                db.OrderDetail.Remove(d);
            }

            // Xóa đơn hàng
            db.OrderPro.Remove(order);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
