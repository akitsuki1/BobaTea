using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using BobaTea.Models;

public class ProduceController : Controller
{
    private readonly BobaTeaEntities db = new BobaTeaEntities();

    // Trang danh sách sản phẩm
    public ActionResult Index()
    {
        var products = db.Products.ToList();
        return View(products);
    }

    // Tìm kiếm đầy đủ (khi ấn Enter)
    public ActionResult Search(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
            return RedirectToAction("Index");

        var results = db.Products
                        .Where(p => p.Name.Contains(keyword) ||
                                    p.Description.Contains(keyword))
                        .ToList();

        ViewBag.Keyword = keyword;

        return View(results);
    }

    // GỢI Ý TÌM KIẾM (cho popup)
    public JsonResult GetSuggest(string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        var data = db.Products
            .Where(p => p.Name.Contains(keyword))
            .Select(p => new
            {
                ProductID = p.ProductId,
                ProductName = p.Name,
                Image = p.ImageUrl,
                Price = p.Price
            })
            .Take(6)
            .ToList();

        return Json(data, JsonRequestBehavior.AllowGet);
    }

    // Trang chi tiết sản phẩm
    public ActionResult Details(int id)
    {
        var product = db.Products.FirstOrDefault(x => x.ProductId == id);
        if (product == null) return HttpNotFound();

        return View(product);
    }
}
