using CHBHTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;


namespace CHBHTH.Controllers
{
    public class sanphamController : Controller
    {
        private QLbanhang db = new QLbanhang();
        // GET: sanpham
        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.Include(s => s.LoaiHang).Include(s => s.NhaCungCap);
            return View(sanPhams.ToList());
        }

        public ActionResult OP()
        {
            var ip = db.SanPhams.Where(n => n.MaLoai == 1).Take(4).ToList();
            return PartialView(ip);
        }
        
            public ActionResult SAS()
        {
            var ip = db.SanPhams.Where(n => n.MaLoai == 2).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult phukien()  
        {
            var ip = db.SanPhams.Where(n => n.MaLoai == 3).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult xemchitiet(int Masp = 0)
        {
            var chitiet = db.SanPhams.SingleOrDefault(n => n.MaSP == Masp);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitiet);
        }

        public ActionResult IP()
        {
            var iphones = db.SanPhams.Where(s => s.LoaiHang.TenLoai == "Iphone").ToList(); // Tùy chỉnh điều kiện lọc sản phẩm của bạn nếu cần
            return View(iphones);
        }

        

       
        public ActionResult xemchitietdanhmuc(int MaLoai)
        {
            var ip = db.SanPhams.Where(n => n.MaLoai == MaLoai).ToList();
            return PartialView(ip);

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
                // Lưu trữ file vào thư mục trên server
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
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