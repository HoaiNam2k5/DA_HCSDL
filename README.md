# 📦 Hệ Thống Quản Lý Kho Hàng Điện Tử (QL_Kho)

Hệ thống quản lý kho hàng điện tử được xây dựng trên nền tảng ASP.NET MVC 5, hỗ trợ quản lý sản phẩm, đơn hàng, người dùng và các nghiệp vụ kinh doanh điện tử.

## 📋 Mục Lục

- [Giới Thiệu](#giới-thiệu)
- [Tính Năng Chính](#tính-năng-chính)
- [Công Nghệ Sử Dụng](#công-nghệ-sử-dụng)
- [Yêu Cầu Hệ Thống](#yêu-cầu-hệ-thống)
- [Hướng Dẫn Cài Đặt](#hướng-dẫn-cài-đặt)
- [Cấu Trúc Thư Mục](#cấu-trúc-thư-mục)
- [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
- [Cơ Sở Dữ Liệu](#cơ-sở-dữ-liệu)
- [Đóng Góp](#đóng-góp)
- [Tác Giả](#tác-giả)

## 🎯 Giới Thiệu

QL_Kho là một hệ thống quản lý kho hàng điện tử chuyên nghiệp, được thiết kế để quản lý các sản phẩm điện thoại di động từ nhiều thương hiệu như iPhone, Samsung, Oppo, Vivo, Xiaomi. Hệ thống cung cấp đầy đủ các chức năng từ quản lý sản phẩm, đặt hàng, thanh toán đến báo cáo thống kê.

## ✨ Tính Năng Chính

### 👤 Dành cho Khách Hàng
- **Đăng ký / Đăng nhập**: Tạo tài khoản và xác thực người dùng
- **Xem sản phẩm**: Duyệt danh sách sản phẩm theo danh mục
- **Tìm kiếm**: Tìm kiếm sản phẩm theo từ khóa
- **Chi tiết sản phẩm**: Xem thông tin chi tiết và sản phẩm liên quan
- **Giỏ hàng**: Thêm, cập nhật, xóa sản phẩm trong giỏ hàng
- **Đặt hàng**: Hoàn tất đơn hàng với thông tin giao hàng
- **Quản lý đơn hàng**: Xem và theo dõi trạng thái đơn hàng
- **Thông tin cá nhân**: Cập nhật thông tin và đổi mật khẩu

### 🔧 Dành cho Quản Trị Viên
- **Dashboard**: Tổng quan thống kê hệ thống
- **Quản lý sản phẩm**: Thêm, sửa, xóa sản phẩm
- **Quản lý đơn hàng**: Cập nhật trạng thái đơn hàng
- **Quản lý người dùng**: Quản lý tài khoản khách hàng
- **Thống kê doanh thu**: Báo cáo doanh thu theo ngày, tháng, năm
- **Sản phẩm bán chạy**: Thống kê các sản phẩm có doanh số cao

### 💡 Tính Năng Đặc Biệt
- **Hỗ trợ trực tuyến**: Chatbox tích hợp sẵn
- **Khuyến mãi**: Popup thông báo khuyến mãi
- **Responsive Design**: Giao diện tương thích đa thiết bị
- **Phân quyền người dùng**: Admin, Quản lý, Khách hàng

## 🛠 Công Nghệ Sử Dụng

### Backend
- **Framework**: ASP.NET MVC 5 (.NET Framework 4.8)
- **ORM**: Entity Framework 6.2
- **Database**: SQL Server

### Frontend
- **HTML5 / CSS3**
- **Bootstrap 5.2.3**
- **jQuery 3.7.0**
- **Font Awesome 6.4.2**
- **Bootstrap Icons 1.11.1**

### Packages & Libraries
- Microsoft.AspNet.Mvc 5.2.9
- Microsoft.AspNet.Razor 3.2.9
- Microsoft.AspNet.WebPages 3.2.9
- Newtonsoft.Json 13.0.3
- jQuery.Validation 1.19.5
- Modernizr 2.8.3
- WebGrease 1.6.0

## 💻 Yêu Cầu Hệ Thống

### Phần Mềm
- **Visual Studio 2022** (phiên bản 17.0 trở lên)
- **SQL Server 2019** trở lên
- **.NET Framework 4.8**
- **IIS Express** hoặc **IIS**

### Phần Cứng Tối Thiểu
- RAM: 4GB
- Ổ cứng: 10GB trống
- CPU: 2 cores

## 📥 Hướng Dẫn Cài Đặt

### Bước 1: Clone Repository
```bash
git clone https://github.com/HoaiNam2k5/DA_HCSDL.git
cd DA_HCSDL
```

### Bước 2: Thiết Lập Database
1. Mở **SQL Server Management Studio (SSMS)**
2. Thực thi file script `Database/QL_KHOScrip.sql` để tạo database và các bảng
3. Script sẽ tự động tạo:
   - Database `DT_DB`
   - Các bảng dữ liệu
   - Stored Procedures
   - Functions
   - Triggers
   - Users và Roles

### Bước 3: Cấu Hình Connection String
1. Mở file `QL_Kho/Web.config`
2. Cập nhật connection string phù hợp với SQL Server của bạn:
```xml
<connectionStrings>
    <add name="Model1" 
         connectionString="data source=YOUR_SERVER;initial catalog=DT_DB;user id=YOUR_USER;password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=True;App=EntityFramework" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Bước 4: Mở Project
1. Mở file `QL_Kho.sln` bằng Visual Studio
2. Đợi Visual Studio restore các NuGet packages tự động
3. Hoặc restore thủ công qua **Tools > NuGet Package Manager > Package Manager Console**:
```powershell
Update-Package -reinstall
```

### Bước 5: Build và Chạy
1. Nhấn **Ctrl + Shift + B** để build project
2. Nhấn **F5** hoặc **Ctrl + F5** để chạy ứng dụng
3. Ứng dụng sẽ chạy tại `https://localhost:44386/`

## 📁 Cấu Trúc Thư Mục

```
DA_HCSDL/
├── Database/
│   └── QL_KHOScrip.sql          # Script tạo database
├── QL_Kho/
│   ├── App_Start/
│   │   ├── BundleConfig.cs      # Cấu hình bundle CSS/JS
│   │   ├── FilterConfig.cs      # Cấu hình filter
│   │   └── RouteConfig.cs       # Cấu hình routing
│   ├── Controllers/
│   │   ├── AccountController.cs # Xử lý đăng nhập/đăng ký
│   │   ├── AdminController.cs   # Chức năng quản trị
│   │   ├── CartController.cs    # Xử lý giỏ hàng
│   │   ├── HomeController.cs    # Trang chủ, sản phẩm
│   │   └── ReportController.cs  # Báo cáo thống kê
│   ├── Filters/
│   │   └── AuthorizeRoleAttribute.cs # Phân quyền
│   ├── Models/
│   │   ├── AUDITLOG.cs          # Nhật ký hệ thống
│   │   ├── CHITIETDONHANG.cs    # Chi tiết đơn hàng
│   │   ├── DANHMUC.cs           # Danh mục sản phẩm
│   │   ├── DONHANG.cs           # Đơn hàng
│   │   ├── GIOHANG.cs           # Giỏ hàng
│   │   ├── NGUOIDUNG.cs         # Người dùng
│   │   ├── NHACUNGCAP.cs        # Nhà cung cấp
│   │   ├── SANPHAM.cs           # Sản phẩm
│   │   ├── Model1.cs            # DbContext
│   │   └── ViewModels/          # Các ViewModel
│   ├── Views/
│   │   ├── Account/             # Views đăng nhập/đăng ký
│   │   ├── Admin/               # Views quản trị
│   │   ├── Cart/                # Views giỏ hàng
│   │   ├── Home/                # Views trang chủ
│   │   └── Shared/              # Layout chung
│   ├── Content/
│   │   ├── images/              # Hình ảnh sản phẩm
│   │   ├── Site.css             # CSS chính
│   │   ├── chatbox.css          # CSS chatbox
│   │   └── promo.css            # CSS khuyến mãi
│   ├── Scripts/
│   │   └── site.js              # JavaScript chính
│   ├── Web.config               # Cấu hình ứng dụng
│   └── packages.config          # Danh sách packages
├── packages/                    # Thư mục NuGet packages
└── QL_Kho.sln                   # Solution file
```

## 📖 Hướng Dẫn Sử Dụng

### Dành Cho Khách Hàng

#### Đăng Ký Tài Khoản
1. Truy cập trang web và nhấn **Đăng ký**
2. Điền đầy đủ thông tin: tên người dùng, email, mật khẩu
3. Nhấn **Đăng ký** để tạo tài khoản

#### Mua Hàng
1. **Đăng nhập** vào hệ thống
2. Duyệt sản phẩm theo **Danh mục** hoặc sử dụng **Tìm kiếm**
3. Nhấn vào sản phẩm để xem chi tiết
4. Nhấn **Thêm vào giỏ hàng**
5. Vào **Giỏ hàng** để kiểm tra
6. Nhấn **Thanh toán**
7. Điền thông tin giao hàng và xác nhận đơn hàng

#### Quản Lý Đơn Hàng
1. Nhấn vào avatar > **Đơn hàng của tôi**
2. Xem danh sách đơn hàng và trạng thái
3. Có thể hủy đơn hàng đang chờ xác nhận

### Dành Cho Quản Trị Viên

#### Truy Cập Admin
1. Đăng nhập với tài khoản có role `admin` hoặc `quanly`
2. Nhấn vào avatar > **Dashboard Admin**

#### Quản Lý Đơn Hàng
1. Vào **Quản lý đơn hàng**
2. Cập nhật trạng thái: Chờ xác nhận → Đang giao → Đã giao

#### Quản Lý Sản Phẩm
1. Vào **Quản lý sản phẩm**
2. Thêm mới hoặc chỉnh sửa thông tin sản phẩm
3. Xóa sản phẩm (soft delete)

## 🗄 Cơ Sở Dữ Liệu

### Các Bảng Chính

| Bảng             | Mô tả                     |
|------------------|---------------------------|
| NGUOIDUNG        | Thông tin người dùng      |
| SANPHAM          | Thông tin sản phẩm        |
| DANHMUC          | Danh mục sản phẩm         |
| NHACUNGCAP       | Thông tin nhà cung cấp    |
| GIOHANG          | Giỏ hàng của khách        |
| DONHANG          | Đơn đặt hàng              |
| CHITIETDONHANG   | Chi tiết đơn hàng         |
| HOADON           | Hóa đơn bán hàng          |
| CTHOADON         | Chi tiết hóa đơn          |
| HOADONNHAPHANG   | Hóa đơn nhập hàng         |
| PHIEUNHAP        | Phiếu nhập kho            |
| AUDITLOG         | Nhật ký thay đổi          |
| BACKUP_HISTORY   | Lịch sử backup            |

### Stored Procedures
- `proc_create_user`: Tạo tài khoản mới
- `sp_TaoDonHang`: Tạo đơn hàng
- `sp_ThemVaoGioHang`: Thêm sản phẩm vào giỏ
- `sp_NhapHang`: Nhập hàng từ nhà cung cấp
- `sp_TaoHoaDonBan`: Tạo hóa đơn bán
- `sp_BackupDatabase`: Backup database
- `sp_LowStockAlert`: Cảnh báo tồn kho thấp

### Functions
- `fun_check_account`: Kiểm tra đăng nhập
- `fun_get_revenue_by_date`: Tính doanh thu theo ngày

## 🤝 Đóng Góp

Chúng tôi luôn chào đón mọi đóng góp! Để đóng góp:

1. Fork repository này
2. Tạo branch mới: `git checkout -b feature/TenTinhNang`
3. Commit thay đổi: `git commit -m 'Thêm tính năng mới'`
4. Push lên branch: `git push origin feature/TenTinhNang`
5. Tạo Pull Request

## 👥 Tác Giả

- **HoaiNam2k5** - *Phát triển chính* - [GitHub](https://github.com/HoaiNam2k5)

## 📄 License

Dự án này được phát triển cho mục đích học tập môn Hệ Cơ Sở Dữ Liệu.

---

<div align="center">
  <p>⭐ Nếu dự án hữu ích, hãy cho chúng tôi một sao! ⭐</p>
  <p>Made with ❤️ by HoaiNam2k5</p>
</div>
