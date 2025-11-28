using System;
using System.Linq;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class CartController : Controller
    {
        private Model1 db = new Model1();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                TempData["Warning"] = "Vui lòng đăng nhập để xem giỏ hàng";
                return RedirectToAction("Login", "Account");
            }

            var userId = Session["UserID"].ToString().Trim();

            var cartItems = db.GIOHANGs
                .Where(g => g.MaUser == userId)
                .OrderByDescending(g => g.NgayThem)
                .ToList();

            ViewBag.TongTien = cartItems.Sum(x => x.SoLuong * x.DonGia);
            ViewBag.SoLuong = cartItems.Sum(x => x.SoLuong);

            return View(cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public JsonResult AddToCart(string maSP, int soLuong = 1)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var userId = Session["UserID"].ToString().Trim();
                maSP = maSP?.Trim();

                // Kiểm tra sản phẩm
                var sanPham = db.SANPHAMs.Find(maSP);
                if (sanPham == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                }

                // Kiểm tra tồn kho
                if (sanPham.SoLuongTon < soLuong)
                {
                    return Json(new { success = false, message = $"Chỉ còn {sanPham.SoLuongTon} sản phẩm" });
                }

                // Kiểm tra đã có trong giỏ chưa
                var existingItem = db.GIOHANGs
                    .FirstOrDefault(g => g.MaUser == userId && g.MaSP == maSP);

                if (existingItem != null)
                {
                    // Kiểm tra tổng số lượng
                    if (sanPham.SoLuongTon < existingItem.SoLuong + soLuong)
                    {
                        return Json(new { success = false, message = "Vượt quá số lượng tồn kho" });
                    }
                    existingItem.SoLuong += soLuong;
                    existingItem.NgayThem = DateTime.Now;
                }
                else
                {
                    var gioHang = new GIOHANG
                    {
                        MaUser = userId,
                        MaSP = maSP,
                        SoLuong = soLuong,
                        DonGia = sanPham.DonGia ?? 0,  
                        NgayThem = DateTime.Now
                    };
                    db.GIOHANGs.Add(gioHang);
                }
                db.SaveChanges();

                // Cập nhật số lượng trong session
                var cartCount = db.GIOHANGs
                    .Where(g => g.MaUser == userId)
                    .Sum(g => (int?)g.SoLuong) ?? 0;

                Session["CartCount"] = cartCount;

                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        // GET: Cart/OrderSuccess
        public ActionResult OrderSuccess(string maDH)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(maDH))
            {
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra đơn hàng có tồn tại không
            var donHang = db.DONHANGs.Find(maDH.Trim());
            if (donHang == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.MaDH = maDH;
            return View();
        }
        // POST: Cart/UpdateCart
        [HttpPost]
        public JsonResult UpdateCart(int maGH, int soLuong)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var userId = Session["UserID"].ToString().Trim();

                var item = db.GIOHANGs.Find(maGH);
                if (item == null || item.MaUser != userId)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                var sanPham = db.SANPHAMs.Find(item.MaSP);
                if (soLuong > sanPham.SoLuongTon)
                {
                    return Json(new { success = false, message = $"Chỉ còn {sanPham.SoLuongTon} sản phẩm" });
                }

                if (soLuong <= 0)
                {
                    db.GIOHANGs.Remove(item);
                }
                else
                {
                    item.SoLuong = soLuong;
                }

                db.SaveChanges();

                var tongTien = db.GIOHANGs
                    .Where(g => g.MaUser == userId)
                    .Sum(g => (decimal?)(g.SoLuong * g.DonGia)) ?? 0;

                var cartCount = db.GIOHANGs
                    .Where(g => g.MaUser == userId)
                    .Sum(g => (int?)g.SoLuong) ?? 0;

                Session["CartCount"] = cartCount;

                return Json(new { success = true, tongTien = tongTien, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        public JsonResult RemoveFromCart(int maGH)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var userId = Session["UserID"].ToString().Trim();

                var item = db.GIOHANGs.Find(maGH);
                if (item != null && item.MaUser == userId)
                {
                    db.GIOHANGs.Remove(item);
                    db.SaveChanges();

                    var cartCount = db.GIOHANGs
                        .Where(g => g.MaUser == userId)
                        .Sum(g => (int?)g.SoLuong) ?? 0;

                    Session["CartCount"] = cartCount;

                    return Json(new { success = true, cartCount = cartCount });
                }

                return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Cart/Checkout
        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
            {
                TempData["Warning"] = "Vui lòng đăng nhập";
                return RedirectToAction("Login", "Account");
            }

            var userId = Session["UserID"].ToString().Trim();

            var cartItems = db.GIOHANGs
                .Where(g => g.MaUser == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Warning"] = "Giỏ hàng trống";
                return RedirectToAction("Index");
            }

            ViewBag.TongTien = cartItems.Sum(x => x.SoLuong * x.DonGia);

            // Lấy thông tin người dùng
            var user = db.NGUOIDUNGs.Find(userId);
            ViewBag.User = user;

            return View(cartItems);
        }

        // POST: Cart/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(string diaChiGiao, string sdt, string ghiChu, string phuongThucTT = "COD")
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var userId = Session["UserID"].ToString().Trim();

                // Kiểm tra giỏ hàng
                var cartItems = db.GIOHANGs.Where(g => g.MaUser == userId).ToList();
                if (!cartItems.Any())
                {
                    return Json(new { success = false, message = "Giỏ hàng trống" });
                }

                // Kiểm tra tồn kho
                foreach (var item in cartItems)
                {
                    var sp = db.SANPHAMs.Find(item.MaSP);
                    if (sp.SoLuongTon < item.SoLuong)
                    {
                        return Json(new { success = false, message = $"Sản phẩm {sp.TenSP} không đủ hàng" });
                    }
                }

                // Tạo đơn hàng
                var tongTien = cartItems.Sum(x => x.SoLuong * x.DonGia);

                // Tạo mã đơn hàng thủ công (vì trigger không hoạt động từ EF)
                var maxMaDH = db.DONHANGs
                    .OrderByDescending(d => d.MaDH)
                    .Select(d => d.MaDH)
                    .FirstOrDefault();

                int nextId = 1;
                if (!string.IsNullOrEmpty(maxMaDH))
                {
                    nextId = int.Parse(maxMaDH.Substring(2)) + 1;
                }
                string maDH = "DH" + nextId.ToString("000");

                var donHang = new DONHANG
                {
                    MaDH = maDH,
                    MaUser = userId,
                    NgayDat = DateTime.Now,
                    TongTien = tongTien,
                    TrangThai = "Chờ xác nhận",
                    DiaChiGiao = diaChiGiao,
                    SDT = sdt,
                    GhiChu = ghiChu,
                    PhuongThucTT = phuongThucTT,
                    NgayCapNhat = DateTime.Now
                };

                db.DONHANGs.Add(donHang);
                db.SaveChanges();

                // Thêm chi tiết đơn hàng
                foreach (var item in cartItems)
                {
                    var chiTiet = new CHITIETDONHANG
                    {
                        MaDH = maDH,
                        MaSP = item.MaSP,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };
                    db.CHITIETDONHANGs.Add(chiTiet);

                    // Giảm tồn kho
                    var sp = db.SANPHAMs.Find(item.MaSP);
                    sp.SoLuongTon -= item.SoLuong;
                    if (sp.SoLuongTon <= 0)
                    {
                        sp.TrangThai = "HetHang";
                    }
                }

                // Xóa giỏ hàng
                db.GIOHANGs.RemoveRange(cartItems);

                db.SaveChanges();

                Session["CartCount"] = 0;

                TempData["Success"] = "Đặt hàng thành công!  Mã đơn hàng: " + maDH;
                return Json(new { success = true, maDH = maDH });
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