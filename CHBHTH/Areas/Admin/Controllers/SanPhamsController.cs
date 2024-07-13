using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CHBHTH.Models;

namespace CHBHTH.Areas.Admin.Controllers
{
    public class SanPhamsController : Controller
    {
        private QLbanhang db = new QLbanhang();

        // GET: SanPhams
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.LoaiHang).Include(s => s.NhaCungCap);
            var u = Session["use"] as CHBHTH.Models.TaiKhoan;
            if (u.PhanQuyen.TenQuyen == "Adminstrator")
            {
                return View(sanPhams.OrderByDescending(s => s.MaSP).ToList());
            }
            return RedirectPermanent("~/Home/Index");
        }

        // GET: SanPhams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }

        // POST: SanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase AnhSP)
        {
            if (ModelState.IsValid)
            {
                if (AnhSP != null && AnhSP.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AnhSP.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    // Kiểm tra và tạo thư mục nếu không tồn tại
                    var directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    AnhSP.SaveAs(path);
                    sanPham.AnhSP = "/Images/" + fileName;
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase AnhSP)
        {
            if (ModelState.IsValid)
            {
                if (AnhSP != null && AnhSP.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AnhSP.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    // Kiểm tra và tạo thư mục nếu không tồn tại
                    var directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    AnhSP.SaveAs(path);
                    sanPham.AnhSP = "/Images/" + fileName;
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.LoaiHangs, "MaLoai", "TenLoai", sanPham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", sanPham.MaNCC);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UploadImage()
        {
            return View();
        }

        // POST: SanPham/UploadImage
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);

                // Kiểm tra và tạo thư mục nếu không tồn tại
                var directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                file.SaveAs(path);

                // Lưu trữ thông tin file vào cơ sở dữ liệu
                var sanPham = new SanPham
                {
                    AnhSP = "/Images/" + fileName,
                    // Thêm các thuộc tính khác của SanPham nếu cần
                };
                db.SanPhams.Add(sanPham);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
