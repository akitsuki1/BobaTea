using BobaTea.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BobaTea.Controllers
{
    public class HomeController : Controller
    {
        private readonly BobaTeaEntities db = new BobaTeaEntities();

        // Trang chủ (Intro)
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Information()
        {
            return View();
        }

        public ActionResult Produce()
        {
            var products = db.Products.ToList();

            return View(products);
        }

        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddToCart(string productName, string imageUrl, int quantity, decimal price, string topping, string sugar, string ice)
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(x => x.ProductName == productName && x.Topping == topping && x.Sugar == sugar && x.Ice == ice);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    ProductName = productName,
                    ImageUrl = imageUrl,
                    Quantity = quantity,
                    Price = price,
                    Topping = topping,
                    Sugar = sugar,
                    Ice = ice
                });

            Session["Cart"] = cart;
            return Json(new { success = true });
        }

        public ActionResult Cart()
        {
            var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
            return View(cart);
        }


        public ActionResult RemoveFromCart(int id)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart != null)
            {
                var item = cart.FirstOrDefault(p => p.ProductId == id);
                if (item != null)
                    cart.Remove(item);

                Session["Cart"] = cart;
            }
            return RedirectToAction("Cart", "Home");
        }

        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}
