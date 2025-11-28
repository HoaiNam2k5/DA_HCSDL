using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using QL_Kho.Models;
using QL_Kho.ViewModels;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute; // FIX: Chỉ định rõ CompareAttribute

namespace QL_Kho.Controllers
{
    public class AccountController : Controller
    {
        private Model1 db = new Model1();

        // GET: Account/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            try
            {
                // Gọi function fun_check_account từ SQL
                var userParam = new SqlParameter("@user", username);
                var passParam = new SqlParameter("@pass", password);

                var result = db.Database.SqlQuery<int>(
                    "SELECT dbo.fun_check_account(@user, @pass)",
                    userParam, passParam
                ).FirstOrDefault();

                if (result == 1)
                {
                    var user = db.NGUOIDUNGs
                        .Where(u => u.TenNguoiDung == username && u.TrangThai == "HoatDong")
                        .FirstOrDefault();

                    if (user != null)
                    {
                        // Lưu session
                        Session["UserID"] = user.MaUser;
                        Session["UserName"] = user.TenNguoiDung;
                        Session["UserRole"] = user.Role;
                        Session["Email"] = user.Email;
                        Session["CartCount"] = 0;

                        TempData["Success"] = $"Chào mừng {user.TenNguoiDung}! ";

                        // Chuyển hướng theo role
                        if (user.Role == "admin" || user.Role == "quanly")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi đăng nhập: " + ex.Message;
                return View();
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra email đã tồn tại
                    var existingUser = db.NGUOIDUNGs.FirstOrDefault(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ViewBag.Error = "Email đã được sử dụng";
                        return View(model);
                    }

                    // Gọi stored procedure proc_create_user
                    var usernameParam = new SqlParameter("@username", model.TenNguoiDung);
                    var passParam = new SqlParameter("@pass", model.MatKhau);
                    var emailParam = new SqlParameter("@email", model.Email);
                    var sdtParam = new SqlParameter("@sdt", string.IsNullOrEmpty(model.SDT) ? (object)DBNull.Value : model.SDT);
                    var diachiParam = new SqlParameter("@diachi", string.IsNullOrEmpty(model.DiaChi) ? (object)DBNull.Value : model.DiaChi);
                    var roleParam = new SqlParameter("@role", "khach");

                    db.Database.ExecuteSqlCommand(
                        "EXEC proc_create_user @username, @pass, @email, @sdt, @diachi, @role",
                        usernameParam, passParam, emailParam, sdtParam, diachiParam, roleParam
                    );

                    TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Email đã tồn tại"))
                    {
                        ViewBag.Error = "Email đã được sử dụng";
                    }
                    else
                    {
                        ViewBag.Error = "Lỗi đăng ký: " + ex.Message;
                    }
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            TempData["Info"] = "Đã đăng xuất thành công";
            return RedirectToAction("Login");
        }

        // GET: Account/ThongTinCaNhan
        public ActionResult ThongTinCaNhan()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            var userId = Session["UserID"].ToString();
            var user = db.NGUOIDUNGs.Find(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ThongTinCaNhanViewModel
            {
                MaUser = user.MaUser,
                TenNguoiDung = user.TenNguoiDung,
                Email = user.Email,
                SDT = user.SDT,
                DiaChi = user.DiaChi
            };

            return View(model);
        }

        // GET: Account/DoiMatKhau
        public ActionResult DoiMatKhau()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        // GET: Account/DonHangCuaToi
        public ActionResult DonHangCuaToi()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            var userId = Session["UserID"].ToString().Trim();
            var orders = db.DONHANGs
                .Where(d => d.MaUser == userId)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(orders);
        }
        // GET: Account/ChiTietDonHang
        public ActionResult ChiTietDonHang(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Mã đơn hàng không hợp lệ";
                return RedirectToAction("DonHangCuaToi");
            }

            var userId = Session["UserID"].ToString().Trim();
            id = id.Trim();

            // Lấy đơn hàng
            var donHang = db.DONHANGs.Find(id);

            if (donHang == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng";
                return RedirectToAction("DonHangCuaToi");
            }

            // Kiểm tra quyền
            if (donHang.MaUser.Trim() != userId)
            {
                TempData["Error"] = "Bạn không có quyền xem đơn hàng này";
                return RedirectToAction("DonHangCuaToi");
            }

            // ✅ SỬA: THÊM NAMESPACE ĐẦY ĐỦ
            var chiTiet = (from ct in db.CHITIETDONHANGs
                           join sp in db.SANPHAMs on ct.MaSP.Trim() equals sp.MaSP.Trim()
                           where ct.MaDH.Trim() == id
                           select new QL_Kho.ViewModels.ChiTietDonHangViewModel  // ✅ THÊM NAMESPACE
                           {
                               MaCTDH = ct.MaCTDH,
                               MaSP = ct.MaSP,
                               TenSP = sp.TenSP,
                               HinhAnh = sp.HinhAnh,
                               SoLuong = ct.SoLuong,
                               DonGia = ct.DonGia,
                               ThanhTien = ct.ThanhTien
                           }).ToList();

            ViewBag.DonHang = donHang;
            return View(chiTiet);
        }

        // POST: Account/HuyDonHang
        [HttpPost]
        public JsonResult HuyDonHang(string maDH)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var userId = Session["UserID"].ToString().Trim();
                maDH = maDH?.Trim();

                var donHang = db.DONHANGs.Find(maDH);

                if (donHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                if (donHang.MaUser.Trim() != userId)
                {
                    return Json(new { success = false, message = "Bạn không có quyền hủy đơn hàng này" });
                }

                if (donHang.TrangThai != "Chờ xác nhận")
                {
                    return Json(new { success = false, message = "Chỉ có thể hủy đơn hàng đang chờ xác nhận" });
                }

                // Hoàn trả số lượng tồn kho
                var chiTiet = db.CHITIETDONHANGs.Where(ct => ct.MaDH.Trim() == maDH).ToList();
                foreach (var item in chiTiet)
                {
                    var sp = db.SANPHAMs.Find(item.MaSP.Trim());
                    if (sp != null)
                    {
                        sp.SoLuongTon += item.SoLuong;
                        if (sp.TrangThai == "HetHang" && sp.SoLuongTon > 0)
                        {
                            sp.TrangThai = "HoatDong";
                        }
                    }
                }

                donHang.TrangThai = "Đã hủy";
                donHang.NgayCapNhat = DateTime.Now;
                db.SaveChanges();

                return Json(new { success = true, message = "Đã hủy đơn hàng thành công" });
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

    #region ViewModels

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên người dùng")]
        public string TenNguoiDung { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")] // FIX: Sử dụng full namespace
        [Display(Name = "Xác nhận mật khẩu")]
        public string XacNhanMatKhau { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
    }

    public class ThongTinCaNhanViewModel
    {
        public string MaUser { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [Display(Name = "Tên người dùng")]
        public string TenNguoiDung { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
    }
// ✅ ViewModel cho chi tiết đơn hàng
public class ChiTietDonHangViewModel
{
    public int MaCTDH { get; set; }
    public string MaSP { get; set; }
    public string TenSP { get; set; }
    public string HinhAnh { get; set; }
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal? ThanhTien { get; set; }
}
public class DoiMatKhauViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string MatKhauCu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string MatKhauMoi { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp")] // FIX
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string XacNhanMatKhauMoi { get; set; }
    }

    #endregion
