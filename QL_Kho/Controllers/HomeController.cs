using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class HomeController : Controller
    {
        private Model1 db = new Model1();

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                // Lấy danh mục
                var danhMucList = db.DANHMUCs
                    .Where(dm => dm.TrangThai == "HoatDong")
                    .OrderBy(dm => dm.MaDM)
                    .ToList();

                foreach (var dm in danhMucList)
                {
                    dm.MaDM = dm.MaDM?.Trim();
                }

                ViewBag.DanhMuc = danhMucList;

                // Lấy 12 sản phẩm mới nhất (tăng từ 8 lên 12)
                var sanPhamMoi = db.SANPHAMs
                    .Where(sp => sp.TrangThai == "HoatDong")
                    .OrderByDescending(sp => sp.NgayTao)
                    .Take(12)  // Thay đổi từ 8 thành 12
                    .ToList();

                return View(sanPhamMoi);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View();
            }
        }
        // GET: Home/ChiTiet/SP001
        public ActionResult ChiTiet(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var sp = db.SANPHAMs.FirstOrDefault(x => x.MaSP == id);

            if (sp == null)
                return HttpNotFound();

            ViewBag.DanhMuc = db.DANHMUCs.FirstOrDefault(d => d.MaDM == sp.MaDM);
            ViewBag.NhaCungCap = db.NHACUNGCAPs.FirstOrDefault(n => n.MaNCC == sp.MaNCC);

            ViewBag.SanPhamLienQuan = db.SANPHAMs
                .Where(x => x.MaDM == sp.MaDM && x.MaSP != sp.MaSP)
                .Take(4)
                .ToList();

            return View(sp);   // 🔥 Quan trọng: phải trả về 1 sản phẩm
        }


        // GET: Home/DanhMuc/DM001
        public ActionResult DanhMuc(string id)
        {
            try
            {
                // Kiểm tra id có null không
                if (string.IsNullOrEmpty(id))
                {
                    // Nếu không có id, hiển thị tất cả sản phẩm
                    ViewBag.TenDanhMuc = "Tất cả sản phẩm";
                    ViewBag.MaDanhMuc = "";

                    var allSanPham = db.SANPHAMs
                        .Where(sp => sp.TrangThai == "HoatDong")
                        .OrderByDescending(sp => sp.NgayTao)
                        .ToList();

                    ViewBag.DanhMucList = db.DANHMUCs
                        .Where(dm => dm.TrangThai == "HoatDong")
                        .ToList();

                    return View(allSanPham);
                }

                // Tìm danh mục theo id
                var danhMuc = db.DANHMUCs.Find(id);

                if (danhMuc == null)
                {
                    TempData["Error"] = "Không tìm thấy danh mục";
                    return RedirectToAction("Index");
                }

                ViewBag.TenDanhMuc = danhMuc.TenDM;
                ViewBag.MaDanhMuc = id;

                // Lấy sản phẩm theo danh mục
                var sanPham = db.SANPHAMs
                    .Where(sp => sp.MaDM == id && sp.TrangThai == "HoatDong")
                    .OrderByDescending(sp => sp.NgayTao)
                    .ToList();

                // Lấy tất cả danh mục cho sidebar
                ViewBag.DanhMucList = db.DANHMUCs
                    .Where(dm => dm.TrangThai == "HoatDong")
                    .ToList();

                return View(sanPham);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        // GET: Home/SanPham
        public ActionResult SanPham()
        {
            try
            {
                // Debug: Đếm tổng số sản phẩm
                var tongSanPham = db.SANPHAMs.Count();
                ViewBag.Debug_TongSanPham = tongSanPham;

                // Debug: Đếm sản phẩm hoạt động
                var sanPhamHoatDong = db.SANPHAMs.Count(sp => sp.TrangThai == "HoatDong");
                ViewBag.Debug_SanPhamHoatDong = sanPhamHoatDong;

                // Lấy tất cả sản phẩm KHÔNG LỌC (để test)
                var sanPham = db.SANPHAMs
                    .OrderByDescending(sp => sp.NgayTao)
                    .ToList();

                ViewBag.Debug_SanPhamLayDuoc = sanPham.Count;

                // Lấy danh mục
                var danhMucList = db.DANHMUCs.ToList();
                ViewBag.DanhMucList = danhMucList;
                ViewBag.MaDanhMuc = "";
                ViewBag.TenDanhMuc = "Tất cả sản phẩm";

                // Log để debug
                System.Diagnostics.Debug.WriteLine($"Tổng sản phẩm: {tongSanPham}");
                System.Diagnostics.Debug.WriteLine($"Hoạt động: {sanPhamHoatDong}");
                System.Diagnostics.Debug.WriteLine($"Lấy được: {sanPham.Count}");

                return View(sanPham);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                ViewBag.ErrorDetail = ex.StackTrace;
                return View(new List<SANPHAM>());
            }
        }

        // GET: Home/TimKiem? keyword=iphone
        public ActionResult TimKiem(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return RedirectToAction("SanPham");
                }

                var sanPham = db.SANPHAMs
                    .Where(sp => sp.TenSP.Contains(keyword) && sp.TrangThai == "HoatDong")
                    .OrderByDescending(sp => sp.NgayTao)
                    .ToList();

                ViewBag.Keyword = keyword;
                ViewBag.SoLuongKetQua = sanPham.Count();

                return View(sanPham);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View();
            }
        }

        // GET: Home/LienHe
        public ActionResult LienHe()
        {
            return View();
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