using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BobaTea.Models;

namespace BobaTea.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BobaTeaEntities db = new BobaTeaEntities();

        // Danh sách sản phẩm
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        // Form thêm mới
        public ActionResult Create()
        {
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // Nếu có upload ảnh
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Content/images/products/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Path.GetFileName(imageFile.FileName);
                    string fullPath = Path.Combine(folderPath, fileName);
                    imageFile.SaveAs(fullPath);

                    // Lưu đường dẫn tương đối vào DB
                    product.ImageUrl = "~/Content/images/products/" + fileName;
                }
                else
                {
                    // Nếu không chọn ảnh
                    product.ImageUrl = "~/Content/images/no-image.png";
                }

                product.CreatedAt = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();

                TempData["Success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            // Xóa file ảnh nếu có (trừ ảnh mặc định)
            if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.Contains("no-image.png"))
            {
                string path = Server.MapPath(product.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            db.Products.Remove(product);
            db.SaveChanges();

            TempData["Success"] = "Đã xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Products product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Products.Find(product.ProductId);
                if (existing == null) return HttpNotFound();

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string folder = Server.MapPath("~/Content/images/products/");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(folder, fileName);
                    imageFile.SaveAs(path);

                    // Cập nhật đường dẫn ảnh mới
                    existing.ImageUrl = "~/Content/images/products/" + fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

    }
}
