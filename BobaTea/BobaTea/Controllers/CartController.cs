using BobaTea.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class CartController : Controller
{
    public ActionResult Index()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        return View(cart);
    }

    [HttpPost]
    public ActionResult AddToCart(int productId, string productName, string imageUrl, decimal price, int quantity,
                              string topping, string sugar, string ice)
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

        var existingItem = cart.FirstOrDefault(x => x.ProductId == productId
            && x.Topping == topping && x.Sugar == sugar && x.Ice == ice);

        if (existingItem != null)
            existingItem.Quantity += quantity;
        else
            cart.Add(new CartItem
            {
                ProductId = productId,
                ProductName = productName,
                ImageUrl = imageUrl,
                Price = price,
                Quantity = quantity,
                Topping = topping,
                Sugar = sugar,
                Ice = ice
            });

        Session["Cart"] = cart;
        return Json(new { success = true });
    }
    public ActionResult Checkout()
    {
        var cart = Session["Cart"] as List<CartItem>;
        if (cart == null || !cart.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống!";
            return RedirectToAction("Index", "Home");
        }
        return View(cart);
    }
    [HttpPost]
    public ActionResult PaymentSuccess()
    {
        Session["Cart"] = null;

        TempData["Success"] = "Thanh toán thành công!";
        return RedirectToAction("Invoice");
    }

    public ActionResult Invoice()
    {
        return View();
    }
}
