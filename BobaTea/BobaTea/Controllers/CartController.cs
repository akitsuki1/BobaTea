using BobaTea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class CartController : Controller
{
    private readonly BobaTeaEntities db = new BobaTeaEntities();


    // Hiển thị giỏ hàng
    public ActionResult Index()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        return View(cart);
    }

    // Thêm sản phẩm vào giỏ hàng
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

    // Hiển thị trang checkout
    public ActionResult Checkout()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống!";
            return RedirectToAction("Index", "Home");
        }
        return View(cart);
    }

    // Thanh toán ngay → hiển thị thông báo thành công
    [HttpPost]
    public ActionResult PaymentSuccess()
    {
        var cart = Session["Cart"] as List<CartItem> ?? new List<CartItem>();


if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng của bạn đang trống!";
            return View("Checkout", cart); // trả về view với model rỗng
        }

        if (Session["CustomerId"] == null)
        {
            TempData["Error"] = "Vui lòng đăng nhập để thanh toán!";
            return RedirectToAction("Login", "Account");
        }

        int customerId = (int)Session["CustomerId"];

        // Tạo đơn hàng mới
        var order = new OrderPro
        {
            DateOrder = DateTime.Now,
            IDCus = customerId
        };
        db.OrderPro.Add(order);
        db.SaveChanges();

        // Thêm chi tiết đơn hàng
        foreach (var item in cart)
        {
            var product = db.Product.Find(item.ProductId);
            if (product == null) continue;

            db.OrderDetail.Add(new OrderDetail
            {
                IDOrder = order.ID,
                IDProduct = item.ProductId,
                Quantity = item.Quantity
            });
        }
        db.SaveChanges();

        // Xóa giỏ hàng
        Session["Cart"] = null;

        // Trả về Checkout kèm thông báo thành công
        TempData["Success"] = "🎉 Thanh toán thành công!";
        return View("Checkout", cart); // model vẫn là danh sách cũ để hiển thị bảng
    } 

}
