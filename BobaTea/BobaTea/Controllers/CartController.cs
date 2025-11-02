using BobaTea.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BobaTea.Models;

public class CartController : Controller
{
    public ActionResult Index()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        return View(cart);
    }

    [HttpPost]
    public JsonResult AddToCart(string productName, string imageUrl, int quantity, decimal price)
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();

        var existing = cart.FirstOrDefault(p => p.ProductName == productName);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductName = productName,
                ImageUrl = imageUrl,
                Quantity = quantity,
                Price = price
            });
        }

        Session["Cart"] = cart;

        return Json(new { success = true });
    }
}
