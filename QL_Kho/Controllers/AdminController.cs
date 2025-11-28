using System;
using System.Linq;
using System.Web.Mvc;
using QL_Kho.Models;
using QL_Kho.ViewModels;

namespace QL_Kho.Controllers
{
    public class AdminController : Controller
    {
        private Model1 db = new Model1();

        // Authorization Filter
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserID"] == null || Session["UserRole"]?.ToString() != "admin")
            {
                TempData["Error"] = "Bạn không có quyền truy cập trang này! ";
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
        // ✅ GET: Admin hoặc Admin/Index
        public ActionResult Index()
        {
            try
            {
                ViewBag.TongDonHang = db.DONHANGs.Count();
                ViewBag.DonHangMoi = db.DONHANGs.Count(d => d.NgayDat >= DateTime.Today);
                ViewBag.TongSanPham = db.SANPHAMs.Count();
                ViewBag.TongNguoiDung = db.NGUOIDUNGs.Count();

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return RedirectToAction("Dashboard");
            }
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                // Thống kê tổng quan
                var stats = new DashboardViewModel
                {
                    // Đơn hàng
                    TongDonHang = db.DONHANGs.Count(),
                    DonHangChoXacNhan = db.DONHANGs.Count(d => d.TrangThai == "Chờ xác nhận"),
                    DonHangDangGiao = db.DONHANGs.Count(d => d.TrangThai == "Đang giao"),
                    DonHangHoanThanh = db.DONHANGs.Count(d => d.TrangThai == "Đã giao"),

                    // Sản phẩm
                    TongSanPham = db.SANPHAMs.Count(),
                    SanPhamHoatDong = db.SANPHAMs.Count(sp => sp.TrangThai == "HoatDong"),
                    SanPhamHetHang = db.SANPHAMs.Count(sp => sp.TrangThai == "HetHang"),

                    // Người dùng
                    TongNguoiDung = db.NGUOIDUNGs.Count(),
                    NguoiDungMoi = db.NGUOIDUNGs.Count(u => u.NgayTao >= DateTime.Now.AddDays(-30)),

                    // Doanh thu
                    DoanhThuHomNay = db.DONHANGs
                        .Where(d => d.NgayDat >= DateTime.Today && d.TrangThai == "Đã giao")
                        .Sum(d => (decimal?)d.TongTien) ?? 0,

                    DoanhThuThangNay = db.DONHANGs
                        .Where(d => d.NgayDat.Month == DateTime.Now.Month &&
                                   d.NgayDat.Year == DateTime.Now.Year &&
                                   d.TrangThai == "Đã giao")
                        .Sum(d => (decimal?)d.TongTien) ?? 0,

                    DoanhThuNamNay = db.DONHANGs
                        .Where(d => d.NgayDat.Year == DateTime.Now.Year && d.TrangThai == "Đã giao")
                        .Sum(d => (decimal?)d.TongTien) ?? 0
                };

                // Đơn hàng mới nhất
                ViewBag.DonHangMoi = db.DONHANGs
                    .OrderByDescending(d => d.NgayDat)
                    .Take(5)
                    .ToList();

                // ✅ SỬA: Sản phẩm bán chạy - Dùng DONHANG thay vì HOADON
                ViewBag.SanPhamBanChay = (from ct in db.CHITIETDONHANGs
                                          join sp in db.SANPHAMs on ct.MaSP.Trim() equals sp.MaSP.Trim()
                                          join dh in db.DONHANGs on ct.MaDH.Trim() equals dh.MaDH.Trim()  // ✅ DÙNG MaDH
                                          where dh.TrangThai == "Đã giao"  // ✅ Chỉ tính đơn đã giao
                                          group ct by new { sp.MaSP, sp.TenSP, sp.HinhAnh, sp.DonGia } into g
                                          orderby g.Sum(x => x.SoLuong) descending
                                          select new SanPhamBanChayViewModel
                                          {
                                              MaSP = g.Key.MaSP,
                                              TenSP = g.Key.TenSP,
                                              HinhAnh = g.Key.HinhAnh,
                                              DonGia = g.Key.DonGia,
                                              SoLuongBan = g.Sum(x => x.SoLuong)
                                          }).Take(5).ToList();

                return View(stats);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi: " + ex.Message;
                return View(new DashboardViewModel());
            }
        }

        // GET: Admin/QuanLySanPham
        public ActionResult QuanLySanPham()
        {
            var sanPham = db.SANPHAMs
                .OrderByDescending(sp => sp.NgayTao)
                .ToList();
            return View(sanPham);
        }

        // GET: Admin/QuanLyDonHang
        public ActionResult QuanLyDonHang()
        {
            var donHang = db.DONHANGs
                .OrderByDescending(d => d.NgayDat)
                .ToList();
            return View(donHang);
        }

        // GET: Admin/QuanLyNguoiDung
        public ActionResult QuanLyNguoiDung()
        {
            var nguoiDung = db.NGUOIDUNGs
                .OrderByDescending(u => u.NgayTao)
                .ToList();
            return View(nguoiDung);
        }

        // POST: Admin/CapNhatTrangThaiDonHang
        [HttpPost]
        public JsonResult CapNhatTrangThaiDonHang(string maDH, string trangThai)
        {
            try
            {
                var donHang = db.DONHANGs.Find(maDH?.Trim());
                if (donHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                donHang.TrangThai = trangThai;
                donHang.NgayCapNhat = DateTime.Now;
                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: Admin/XoaSanPham
        [HttpPost]
        public JsonResult XoaSanPham(string maSP)
        {
            try
            {
                var sanPham = db.SANPHAMs.Find(maSP?.Trim());
                if (sanPham == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                // Soft delete
                sanPham.TrangThai = "KhongHoatDong";
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
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