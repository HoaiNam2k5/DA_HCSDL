USE master;
GO

/* ============================================================
   PHẦN 1: TẠO VÀ CẤU HÌNH DATABASE
============================================================ */
-- Tạo database mới
CREATE DATABASE DT_DB
ON PRIMARY 
(
    NAME = DT_DB_Data,
    FILENAME = 'C:\SQLData\DT_DB. mdf',
    SIZE = 100MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 10MB
)
LOG ON
(
    NAME = DT_DB_Log,
    FILENAME = 'C:\SQLData\DT_DB_log.ldf',
    SIZE = 50MB,
    MAXSIZE = 500MB,
    FILEGROWTH = 5MB
);
GO

-- Cấu hình Recovery Model cho sao lưu
ALTER DATABASE DT_DB SET RECOVERY FULL;
GO

USE DT_DB;
GO

PRINT N'========== ĐÃ TẠO DATABASE DT_DB THÀNH CÔNG ==========';
GO

/* ============================================================
   PHẦN 2: TẠO CÁC BẢNG DỮ LIỆU
   CLO1.1 & CLO1.2: Sử dụng lệnh T-SQL và các thành phần CSDL
============================================================ */

-- Bảng NGƯỜI DÙNG
CREATE TABLE NGUOIDUNG (
    MaUser CHAR(10) PRIMARY KEY,
    TenNguoiDung NVARCHAR(100) NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(200),
    Role VARCHAR(20) NOT NULL CHECK (Role IN ('admin', 'quanly', 'khach')),
    TrangThai VARCHAR(20) DEFAULT 'HoatDong' CHECK (TrangThai IN ('HoatDong', 'KhongHoatDong')),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- Bảng NHÀ CUNG CẤP
CREATE TABLE NHACUNGCAP (
    MaNCC CHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(100) NOT NULL,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(100),
    Email VARCHAR(100),
    TrangThai VARCHAR(20) DEFAULT 'HoatDong',
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng DANH MỤC
CREATE TABLE DANHMUC (
    MaDM CHAR(10) PRIMARY KEY,
    TenDM NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),
    HinhAnh NVARCHAR(255),
    TrangThai VARCHAR(20) DEFAULT 'HoatDong',
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng SẢN PHẨM
CREATE TABLE SANPHAM (
    MaSP CHAR(10) PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    DonGia DECIMAL(18,2) CHECK (DonGia >= 0),
    DonViTinh NVARCHAR(20),
    SoLuongTon INT DEFAULT 0 CHECK (SoLuongTon >= 0),
    HinhAnh NVARCHAR(255),
    MoTa NVARCHAR(1000),
    MaNCC CHAR(10),
    MaDM CHAR(10),
    TrangThai VARCHAR(20) DEFAULT 'HoatDong' CHECK (TrangThai IN ('HoatDong', 'KhongHoatDong', 'HetHang')),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);
GO

-- Bảng HÓA ĐƠN NHẬP HÀNG
CREATE TABLE HOADONNHAPHANG (
    MaHDN CHAR(10) PRIMARY KEY,
    NgayNhap DATE NOT NULL,
    TongTien DECIMAL(18,2) DEFAULT 0 CHECK (TongTien >= 0),
    MaNCC CHAR(10) NOT NULL,
    MaQL CHAR(10) NOT NULL,
    GhiChu NVARCHAR(500),
    TrangThai VARCHAR(20) DEFAULT 'DaHoanThanh',
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng PHIẾU NHẬP (Chi tiết hóa đơn nhập)
CREATE TABLE PHIEUNHAP (
    MaHDN CHAR(10),
    MaSP CHAR(10),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18,2) NOT NULL CHECK (DonGiaNhap >= 0),
    ThanhTien AS (SoLuong * DonGiaNhap) PERSISTED,
    PRIMARY KEY (MaHDN, MaSP)
);
GO

-- Bảng HÓA ĐƠN BÁN
CREATE TABLE HOADON (
    MaHD CHAR(10) PRIMARY KEY,
    NgayLap DATE NOT NULL,
    TongTien DECIMAL(18,2) DEFAULT 0 CHECK (TongTien >= 0),
    TrangThai NVARCHAR(30) DEFAULT N'Chờ xử lý' CHECK (TrangThai IN (N'Chờ xử lý', N'Đã thanh toán', N'Đã hủy')),
    PhuongThucTT NVARCHAR(20) CHECK (PhuongThucTT IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ')),
    MaKH CHAR(10),
    MaQL CHAR(10),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- Bảng CHI TIẾT HÓA ĐƠN BÁN
CREATE TABLE CTHOADON (
    MaHD CHAR(10),
    MaSP CHAR(10),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    ThanhTien AS (SoLuong * DonGia) PERSISTED,
    PRIMARY KEY (MaHD, MaSP)
);
GO

-- Bảng AUDIT LOG - Theo dõi hoạt động hệ thống
CREATE TABLE AUDITLOG (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    TableName NVARCHAR(50),
    Operation NVARCHAR(20),
    MaUser CHAR(10),
    NgayThucHien DATETIME DEFAULT GETDATE(),
    NoiDung NVARCHAR(MAX)
);
GO

-- Bảng lịch sử sao lưu
CREATE TABLE BACKUP_HISTORY (
    BackupID INT IDENTITY(1,1) PRIMARY KEY,
    BackupType NVARCHAR(20), -- 'FULL', 'DIFFERENTIAL', 'LOG'
    BackupPath NVARCHAR(500),
    BackupSize BIGINT,
    StartTime DATETIME,
    EndTime DATETIME,
    Status NVARCHAR(20), -- 'Success', 'Failed'
    ErrorMessage NVARCHAR(MAX)
);
GO

PRINT N'========== ĐÃ TẠO TẤT CẢ CÁC BẢNG ==========';
GO

/* ============================================================
   PHẦN 3: TẠO KHÓA NGOẠI
============================================================ */

ALTER TABLE SANPHAM
ADD CONSTRAINT FK_SANPHAM_NCC FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP(MaNCC),
    CONSTRAINT FK_SANPHAM_DM FOREIGN KEY (MaDM) REFERENCES DANHMUC(MaDM);
GO

ALTER TABLE HOADON
ADD CONSTRAINT FK_HOADON_KH FOREIGN KEY (MaKH) REFERENCES NGUOIDUNG(MaUser),
    CONSTRAINT FK_HOADON_QL FOREIGN KEY (MaQL) REFERENCES NGUOIDUNG(MaUser);
GO

ALTER TABLE CTHOADON
ADD CONSTRAINT FK_CTHD_HD FOREIGN KEY (MaHD) REFERENCES HOADON(MaHD) ON DELETE CASCADE,
    CONSTRAINT FK_CTHD_SP FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP);
GO

ALTER TABLE HOADONNHAPHANG
ADD CONSTRAINT FK_HDN_NCC FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP(MaNCC),
    CONSTRAINT FK_HDN_QL FOREIGN KEY (MaQL) REFERENCES NGUOIDUNG(MaUser);
GO

ALTER TABLE PHIEUNHAP
ADD CONSTRAINT FK_PN_HDN FOREIGN KEY (MaHDN) REFERENCES HOADONNHAPHANG(MaHDN) ON DELETE CASCADE,
    CONSTRAINT FK_PN_SP FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP);
GO

PRINT N'========== ĐÃ TẠO KHÓA NGOẠI ==========';
GO

/* ============================================================
   PHẦN 4: TẠO TRIGGERS - TỰ ĐỘNG SINH MÃ
   CLO1.2: Viết trigger theo yêu cầu chi tiết
============================================================ */

-- Xóa triggers cũ nếu tồn tại
IF OBJECT_ID('trg_AutoID_NGUOIDUNG', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_NGUOIDUNG;
IF OBJECT_ID('trg_AutoID_NHACUNGCAP', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_NHACUNGCAP;
IF OBJECT_ID('trg_AutoID_DANHMUC', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_DANHMUC;
IF OBJECT_ID('trg_AutoID_SANPHAM', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_SANPHAM;
IF OBJECT_ID('trg_AutoID_HDN', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_HDN;
IF OBJECT_ID('trg_AutoID_HD', 'TR') IS NOT NULL DROP TRIGGER trg_AutoID_HD;
IF OBJECT_ID('trg_UpdateSoLuongTonSauNhap', 'TR') IS NOT NULL DROP TRIGGER trg_UpdateSoLuongTonSauNhap;
IF OBJECT_ID('trg_CheckAndUpdateSoLuongTonSauBan', 'TR') IS NOT NULL DROP TRIGGER trg_CheckAndUpdateSoLuongTonSauBan;
IF OBJECT_ID('trg_RestoreSoLuongWhenDelete', 'TR') IS NOT NULL DROP TRIGGER trg_RestoreSoLuongWhenDelete;
GO

-- TRIGGER 1: Tự sinh mã NGUOIDUNG
CREATE TRIGGER trg_AutoID_NGUOIDUNG
ON NGUOIDUNG
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaUser, 3, 3) AS INT)), 0) FROM NGUOIDUNG);
    INSERT INTO NGUOIDUNG (MaUser, TenNguoiDung, MatKhau, Email, SDT, DiaChi, Role, TrangThai, NgayTao)
    SELECT 
        'US' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        TenNguoiDung, MatKhau, Email, SDT, DiaChi, Role,
        ISNULL(TrangThai, 'HoatDong'), GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 2: Tự sinh mã NHACUNGCAP
CREATE TRIGGER trg_AutoID_NHACUNGCAP
ON NHACUNGCAP
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaNCC, 4, 3) AS INT)), 0) FROM NHACUNGCAP);
    INSERT INTO NHACUNGCAP (MaNCC, TenNCC, SDT, DiaChi, Email, TrangThai, NgayTao)
    SELECT 
        'NCC' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        TenNCC, SDT, DiaChi, Email, ISNULL(TrangThai, 'HoatDong'), GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 3: Tự sinh mã DANHMUC
CREATE TRIGGER trg_AutoID_DANHMUC
ON DANHMUC
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaDM, 3, 3) AS INT)), 0) FROM DANHMUC);
    INSERT INTO DANHMUC (MaDM, TenDM, MoTa, HinhAnh, TrangThai, NgayTao)
    SELECT 
        'DM' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        TenDM, MoTa, HinhAnh, ISNULL(TrangThai, 'HoatDong'), GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 4: Tự sinh mã SANPHAM
CREATE TRIGGER trg_AutoID_SANPHAM
ON SANPHAM
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaSP, 3, 3) AS INT)), 0) FROM SANPHAM);
    INSERT INTO SANPHAM (MaSP, TenSP, DonGia, DonViTinh, SoLuongTon, HinhAnh, MoTa, MaNCC, MaDM, TrangThai, NgayTao)
    SELECT 
        'SP' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        TenSP, DonGia, DonViTinh, ISNULL(SoLuongTon, 0), HinhAnh, MoTa, MaNCC, MaDM, 
        ISNULL(TrangThai, 'HoatDong'), GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 5: Tự sinh mã HOADONNHAPHANG
CREATE TRIGGER trg_AutoID_HDN
ON HOADONNHAPHANG
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaHDN, 4, 3) AS INT)), 0) FROM HOADONNHAPHANG);
    INSERT INTO HOADONNHAPHANG (MaHDN, NgayNhap, TongTien, MaNCC, MaQL, GhiChu, TrangThai, NgayTao)
    SELECT 
        'HDN' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        NgayNhap, ISNULL(TongTien, 0), MaNCC, MaQL, GhiChu, 
        ISNULL(TrangThai, 'DaHoanThanh'), GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 6: Tự sinh mã HOADON
CREATE TRIGGER trg_AutoID_HD
ON HOADON
INSTEAD OF INSERT
AS
BEGIN
    DECLARE @maxID INT = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaHD, 3, 3) AS INT)), 0) FROM HOADON);
    INSERT INTO HOADON (MaHD, NgayLap, TongTien, TrangThai, PhuongThucTT, MaKH, MaQL, GhiChu, NgayTao)
    SELECT 
        'HD' + RIGHT('000' + CAST(@maxID + ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS VARCHAR(3)), 3),
        NgayLap, ISNULL(TongTien, 0), ISNULL(TrangThai, N'Chờ xử lý'), 
        PhuongThucTT, MaKH, MaQL, GhiChu, GETDATE()
    FROM inserted;
END;
GO

-- TRIGGER 7: Cập nhật số lượng tồn sau nhập hàng (TRANSACTION + CURSOR logic)
CREATE TRIGGER trg_UpdateSoLuongTonSauNhap
ON PHIEUNHAP
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Cập nhật số lượng tồn
        UPDATE s
        SET s.SoLuongTon = s.SoLuongTon + i.SoLuong,
            s.NgayCapNhat = GETDATE(),
            s.TrangThai = CASE 
                WHEN s.TrangThai = 'HetHang' THEN 'HoatDong'
                ELSE s.TrangThai
            END
        FROM SANPHAM s
        INNER JOIN inserted i ON s.MaSP = i.MaSP;

        -- Cập nhật tổng tiền hóa đơn nhập
        UPDATE h
        SET h.TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGiaNhap), 0)
            FROM PHIEUNHAP
            WHERE MaHDN = h.MaHDN
        )
        FROM HOADONNHAPHANG h
        INNER JOIN inserted i ON h.MaHDN = i.MaHDN;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- TRIGGER 8: Kiểm tra tồn kho và cập nhật khi bán (TRANSACTION)
CREATE TRIGGER trg_CheckAndUpdateSoLuongTonSauBan
ON CTHOADON
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra số lượng tồn kho
        IF EXISTS (
            SELECT 1
            FROM inserted i
            INNER JOIN SANPHAM s ON i.MaSP = s.MaSP
            WHERE s.SoLuongTon < i.SoLuong
        )
        BEGIN
            RAISERROR(N'Số lượng tồn kho không đủ', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Cập nhật số lượng tồn
        UPDATE s
        SET s. SoLuongTon = s.SoLuongTon - i.SoLuong,
            s.NgayCapNhat = GETDATE(),
            s.TrangThai = CASE 
                WHEN (s.SoLuongTon - i.SoLuong) <= 0 THEN 'HetHang'
                ELSE s.TrangThai
            END
        FROM SANPHAM s
        INNER JOIN inserted i ON s.MaSP = i. MaSP;

        -- Cập nhật tổng tiền hóa đơn
        UPDATE h
        SET h. TongTien = (
            SELECT ISNULL(SUM(SoLuong * DonGia), 0)
            FROM CTHOADON
            WHERE MaHD = h.MaHD
        )
        FROM HOADON h
        INNER JOIN inserted i ON h.MaHD = i.MaHD;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- TRIGGER 9: Hoàn trả số lượng khi xóa chi tiết hóa đơn
CREATE TRIGGER trg_RestoreSoLuongWhenDelete
ON CTHOADON
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE s
    SET s.SoLuongTon = s. SoLuongTon + d.SoLuong,
        s.NgayCapNhat = GETDATE(),
        s.TrangThai = CASE 
            WHEN s.TrangThai = 'HetHang' THEN 'HoatDong'
            ELSE s.TrangThai
        END
    FROM SANPHAM s
    INNER JOIN deleted d ON s.MaSP = d.MaSP;

    UPDATE h
    SET h.TongTien = (
        SELECT ISNULL(SUM(SoLuong * DonGia), 0)
        FROM CTHOADON
        WHERE MaHD = h.MaHD
    )
    FROM HOADON h
    INNER JOIN deleted d ON h. MaHD = d.MaHD;
END;
GO

PRINT N'========== ĐÃ TẠO 9 TRIGGERS ==========';
GO

/* ============================================================
   PHẦN 5: TẠO STORED PROCEDURES
   CLO1.2: Cài đặt package quản lý người dùng
============================================================ */

-- Xóa procedures cũ
IF OBJECT_ID('proc_create_user', 'P') IS NOT NULL DROP PROCEDURE proc_create_user;
IF OBJECT_ID('proc_alter_user', 'P') IS NOT NULL DROP PROCEDURE proc_alter_user;
IF OBJECT_ID('proc_delete_user', 'P') IS NOT NULL DROP PROCEDURE proc_delete_user;
IF OBJECT_ID('sp_BaoCaoDoanhThu', 'P') IS NOT NULL DROP PROCEDURE sp_BaoCaoDoanhThu;
IF OBJECT_ID('sp_BaoCaoSanPhamBanChay', 'P') IS NOT NULL DROP PROCEDURE sp_BaoCaoSanPhamBanChay;
IF OBJECT_ID('sp_BaoCaoTonKho', 'P') IS NOT NULL DROP PROCEDURE sp_BaoCaoTonKho;
IF OBJECT_ID('sp_CheckLowStock', 'P') IS NOT NULL DROP PROCEDURE sp_CheckLowStock;
IF OBJECT_ID('sp_BackupDatabase', 'P') IS NOT NULL DROP PROCEDURE sp_BackupDatabase;
IF OBJECT_ID('sp_NhapHang', 'P') IS NOT NULL DROP PROCEDURE sp_NhapHang;
IF OBJECT_ID('sp_TaoHoaDonBan', 'P') IS NOT NULL DROP PROCEDURE sp_TaoHoaDonBan;
GO

-- PROCEDURE 1: Tạo người dùng mới (TRANSACTION)
CREATE PROCEDURE proc_create_user
    @username NVARCHAR(100),
    @pass NVARCHAR(255),
    @email VARCHAR(100),
    @sdt VARCHAR(15) = NULL,
    @diachi NVARCHAR(200) = NULL,
    @role VARCHAR(20) = 'khach'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM NGUOIDUNG WHERE Email = @email)
        BEGIN
            RAISERROR(N'Email đã tồn tại', 16, 1);
            RETURN;
        END

        INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, Email, SDT, DiaChi, Role)
        VALUES (@username, @pass, @email, @sdt, @diachi, @role);

        INSERT INTO AUDITLOG (TableName, Operation, NoiDung)
        VALUES ('NGUOIDUNG', 'INSERT', N'Tạo người dùng: ' + @username);

        COMMIT TRANSACTION;
        PRINT N'Tạo người dùng thành công';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE 2: Sửa thông tin người dùng (TRANSACTION)
CREATE PROCEDURE proc_alter_user
    @username NVARCHAR(100),
    @pass NVARCHAR(255) = NULL,
    @email VARCHAR(100) = NULL,
    @sdt VARCHAR(15) = NULL,
    @diachi NVARCHAR(200) = NULL,
    @role VARCHAR(20) = NULL,
    @trangthai VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @MaUser CHAR(10);
        SELECT @MaUser = MaUser FROM NGUOIDUNG WHERE TenNguoiDung = @username;
        
        IF @MaUser IS NULL
        BEGIN
            RAISERROR(N'Không tìm thấy người dùng', 16, 1);
            RETURN;
        END

        UPDATE NGUOIDUNG
        SET 
            MatKhau = ISNULL(@pass, MatKhau),
            Email = ISNULL(@email, Email),
            SDT = ISNULL(@sdt, SDT),
            DiaChi = ISNULL(@diachi, DiaChi),
            Role = ISNULL(@role, Role),
            TrangThai = ISNULL(@trangthai, TrangThai),
            NgayCapNhat = GETDATE()
        WHERE MaUser = @MaUser;

        INSERT INTO AUDITLOG (TableName, Operation, MaUser, NoiDung)
        VALUES ('NGUOIDUNG', 'UPDATE', @MaUser, N'Cập nhật: ' + @username);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE 3: Xóa người dùng (soft delete)
CREATE PROCEDURE proc_delete_user
    @username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        UPDATE NGUOIDUNG
        SET TrangThai = 'KhongHoatDong', NgayCapNhat = GETDATE()
        WHERE TenNguoiDung = @username;

        INSERT INTO AUDITLOG (TableName, Operation, NoiDung)
        VALUES ('NGUOIDUNG', 'DELETE', N'Vô hiệu hóa: ' + @username);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE 4: Báo cáo doanh thu
CREATE PROCEDURE sp_BaoCaoDoanhThu
    @TuNgay DATE,
    @DenNgay DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CONVERT(DATE, NgayLap) AS Ngay,
        COUNT(*) AS SoLuongHoaDon,
        SUM(TongTien) AS TongDoanhThu,
        AVG(TongTien) AS DoanhThuTrungBinh
    FROM HOADON
    WHERE NgayLap BETWEEN @TuNgay AND @DenNgay
        AND TrangThai = N'Đã thanh toán'
    GROUP BY CONVERT(DATE, NgayLap)
    ORDER BY Ngay DESC;
END;
GO

-- PROCEDURE 5: Báo cáo sản phẩm bán chạy
CREATE PROCEDURE sp_BaoCaoSanPhamBanChay
    @Top INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Top)
        s.MaSP, s.TenSP, dm.TenDM,
        SUM(ct.SoLuong) AS TongSoLuongBan,
        SUM(ct. SoLuong * ct.DonGia) AS TongDoanhThu,
        s.SoLuongTon
    FROM CTHOADON ct
    INNER JOIN SANPHAM s ON ct.MaSP = s.MaSP
    INNER JOIN DANHMUC dm ON s.MaDM = dm.MaDM
    INNER JOIN HOADON h ON ct.MaHD = h.MaHD
    WHERE h.TrangThai = N'Đã thanh toán'
    GROUP BY s.MaSP, s.TenSP, dm.TenDM, s.SoLuongTon
    ORDER BY TongSoLuongBan DESC;
END;
GO

-- PROCEDURE 6: Báo cáo tồn kho
CREATE PROCEDURE sp_BaoCaoTonKho
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        dm.TenDM, s.MaSP, s.TenSP, s.SoLuongTon, s.DonGia,
        s.SoLuongTon * s.DonGia AS GiaTriTonKho,
        s.TrangThai
    FROM SANPHAM s
    INNER JOIN DANHMUC dm ON s.MaDM = dm.MaDM
    WHERE s.TrangThai IN ('HoatDong', 'HetHang')
    ORDER BY dm.TenDM, s. TenSP;
END;
GO

-- PROCEDURE 7: Kiểm tra sản phẩm sắp hết hàng (CURSOR)
CREATE PROCEDURE sp_CheckLowStock
    @SoLuongToiThieu INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @MaSP CHAR(10), @TenSP NVARCHAR(100), @SoLuongTon INT, @Message NVARCHAR(500);
    
    CREATE TABLE #LowStockProducts (
        MaSP CHAR(10), TenSP NVARCHAR(100), 
        SoLuongTon INT, ThongBao NVARCHAR(500)
    );
    
    DECLARE cur_LowStock CURSOR FOR
    SELECT MaSP, TenSP, SoLuongTon
    FROM SANPHAM
    WHERE SoLuongTon <= @SoLuongToiThieu AND TrangThai = 'HoatDong'
    ORDER BY SoLuongTon ASC;
    
    OPEN cur_LowStock;
    FETCH NEXT FROM cur_LowStock INTO @MaSP, @TenSP, @SoLuongTon;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Message = N'Cảnh báo: "' + @TenSP + N'" còn ' + CAST(@SoLuongTon AS NVARCHAR(10)) + N' sản phẩm';
        INSERT INTO #LowStockProducts VALUES (@MaSP, @TenSP, @SoLuongTon, @Message);
        FETCH NEXT FROM cur_LowStock INTO @MaSP, @TenSP, @SoLuongTon;
    END;
    
    CLOSE cur_LowStock;
    DEALLOCATE cur_LowStock;
    
    SELECT * FROM #LowStockProducts;
    DROP TABLE #LowStockProducts;
END;
GO

-- PROCEDURE 8: Sao lưu database (TRANSACTION)
CREATE PROCEDURE sp_BackupDatabase
    @BackupType VARCHAR(20) = 'FULL', -- 'FULL', 'DIFFERENTIAL', 'LOG'
    @BackupPath NVARCHAR(500) = 'C:\SQLBackup\'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @BackupFile NVARCHAR(500);
        DECLARE @DateTime VARCHAR(20) = CONVERT(VARCHAR(20), GETDATE(), 112) + '_' + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 108), ':', '');
        DECLARE @StartTime DATETIME = GETDATE();
        DECLARE @SQL NVARCHAR(MAX);
        
        IF @BackupType = 'FULL'
        BEGIN
            SET @BackupFile = @BackupPath + 'DT_DB_FULL_' + @DateTime + '.bak';
            SET @SQL = 'BACKUP DATABASE DT_DB TO DISK = ''' + @BackupFile + ''' WITH INIT, FORMAT, NAME = ''Full Backup''';
        END
        ELSE IF @BackupType = 'DIFFERENTIAL'
        BEGIN
            SET @BackupFile = @BackupPath + 'DT_DB_DIFF_' + @DateTime + '.bak';
            SET @SQL = 'BACKUP DATABASE DT_DB TO DISK = ''' + @BackupFile + ''' WITH DIFFERENTIAL, INIT, FORMAT';
        END
        ELSE IF @BackupType = 'LOG'
        BEGIN
            SET @BackupFile = @BackupPath + 'DT_DB_LOG_' + @DateTime + '. trn';
            SET @SQL = 'BACKUP LOG DT_DB TO DISK = ''' + @BackupFile + ''' WITH INIT, FORMAT';
        END
        
        EXEC sp_executesql @SQL;
        
        INSERT INTO BACKUP_HISTORY (BackupType, BackupPath, StartTime, EndTime, Status)
        VALUES (@BackupType, @BackupFile, @StartTime, GETDATE(), 'Success');
        
        PRINT N'Sao lưu thành công: ' + @BackupFile;
    END TRY
    BEGIN CATCH
        INSERT INTO BACKUP_HISTORY (BackupType, BackupPath, StartTime, EndTime, Status, ErrorMessage)
        VALUES (@BackupType, @BackupFile, @StartTime, GETDATE(), 'Failed', ERROR_MESSAGE());
        THROW;
    END CATCH
END;
GO

-- PROCEDURE 9: Nhập hàng (TRANSACTION + TRIGGER)
CREATE PROCEDURE sp_NhapHang
    @MaNCC CHAR(10),
    @MaQL CHAR(10),
    @ChiTietNhap NVARCHAR(MAX) -- JSON: [{"MaSP":"SP001","SoLuong":100,"DonGiaNhap":50000}]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Tạo hóa đơn nhập
        DECLARE @MaHDN CHAR(10);
        INSERT INTO HOADONNHAPHANG (NgayNhap, MaNCC, MaQL)
        VALUES (GETDATE(), @MaNCC, @MaQL);
        
        SET @MaHDN = (SELECT TOP 1 MaHDN FROM HOADONNHAPHANG ORDER BY NgayTao DESC);
        
        -- Thêm chi tiết (sử dụng JSON)
        INSERT INTO PHIEUNHAP (MaHDN, MaSP, SoLuong, DonGiaNhap)
        SELECT @MaHDN, MaSP, SoLuong, DonGiaNhap
        FROM OPENJSON(@ChiTietNhap)
        WITH (
            MaSP CHAR(10) '$.MaSP',
            SoLuong INT '$.SoLuong',
            DonGiaNhap DECIMAL(18,2) '$.DonGiaNhap'
        );
        
        -- Trigger tự động cập nhật tồn kho và tổng tiền
        
        INSERT INTO AUDITLOG (TableName, Operation, NoiDung)
        VALUES ('HOADONNHAPHANG', 'INSERT', N'Nhập hàng: ' + @MaHDN);
        
        COMMIT TRANSACTION;
        SELECT @MaHDN AS MaHDN, N'Nhập hàng thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- PROCEDURE 10: Tạo hóa đơn bán (TRANSACTION + TRIGGER)
CREATE PROCEDURE sp_TaoHoaDonBan
    @MaKH CHAR(10),
    @MaQL CHAR(10),
    @PhuongThucTT NVARCHAR(20),
    @ChiTietBan NVARCHAR(MAX) -- JSON: [{"MaSP":"SP001","SoLuong":2,"DonGia":1000000}]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Tạo hóa đơn bán
        DECLARE @MaHD CHAR(10);
        INSERT INTO HOADON (NgayLap, PhuongThucTT, MaKH, MaQL, TrangThai)
        VALUES (GETDATE(), @PhuongThucTT, @MaKH, @MaQL, N'Đã thanh toán');
        
        SET @MaHD = (SELECT TOP 1 MaHD FROM HOADON ORDER BY NgayTao DESC);
        
        -- Thêm chi tiết
        INSERT INTO CTHOADON (MaHD, MaSP, SoLuong, DonGia)
        SELECT @MaHD, MaSP, SoLuong, DonGia
        FROM OPENJSON(@ChiTietBan)
        WITH (
            MaSP CHAR(10) '$.MaSP',
            SoLuong INT '$.SoLuong',
            DonGia DECIMAL(18,2) '$.DonGia'
        );
        
        -- Trigger tự động kiểm tra tồn kho và cập nhật
        
        INSERT INTO AUDITLOG (TableName, Operation, NoiDung)
        VALUES ('HOADON', 'INSERT', N'Bán hàng: ' + @MaHD);
        
        COMMIT TRANSACTION;
        SELECT @MaHD AS MaHD, N'Tạo hóa đơn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

PRINT N'========== ĐÃ TẠO 10 STORED PROCEDURES ==========';
GO

/* ============================================================
   PHẦN 6: TẠO FUNCTIONS
============================================================ */

IF OBJECT_ID('fun_check_account', 'FN') IS NOT NULL DROP FUNCTION fun_check_account;
IF OBJECT_ID('fun_get_revenue_by_date', 'FN') IS NOT NULL DROP FUNCTION fun_get_revenue_by_date;
GO

-- FUNCTION 1: Kiểm tra tài khoản
CREATE FUNCTION fun_check_account(@user NVARCHAR(100), @pass NVARCHAR(255))
RETURNS INT
AS
BEGIN
    DECLARE @result INT = 0;
    IF EXISTS (
        SELECT 1 FROM NGUOIDUNG 
        WHERE TenNguoiDung = @user AND MatKhau = @pass AND TrangThai = 'HoatDong'
    )
        SET @result = 1;
    RETURN @result;
END;
GO

-- FUNCTION 2: Tính doanh thu theo ngày
CREATE FUNCTION fun_get_revenue_by_date(@Ngay DATE)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @DoanhThu DECIMAL(18,2);
    SELECT @DoanhThu = ISNULL(SUM(TongTien), 0)
    FROM HOADON
    WHERE CONVERT(DATE, NgayLap) = @Ngay AND TrangThai = N'Đã thanh toán';
    RETURN @DoanhThu;
END;
GO

PRINT N'========== ĐÃ TẠO 2 FUNCTIONS ==========';
GO

/* ============================================================
   PHẦN 7: TẠO VIEWS
============================================================ */

IF OBJECT_ID('vw_SanPhamDayDu', 'V') IS NOT NULL DROP VIEW vw_SanPhamDayDu;
IF OBJECT_ID('vw_HoaDonChiTiet', 'V') IS NOT NULL DROP VIEW vw_HoaDonChiTiet;
GO

-- VIEW 1: Sản phẩm đầy đủ
CREATE VIEW vw_SanPhamDayDu
AS
SELECT 
    s.MaSP, s.TenSP, s.DonGia, s.DonViTinh, s.SoLuongTon, s.HinhAnh, s.MoTa,
    dm.TenDM AS DanhMuc, ncc.TenNCC AS NhaCungCap, s.TrangThai
FROM SANPHAM s
LEFT JOIN DANHMUC dm ON s.MaDM = dm.MaDM
LEFT JOIN NHACUNGCAP ncc ON s.MaNCC = ncc. MaNCC;
GO

-- VIEW 2: Hóa đơn chi tiết
CREATE VIEW vw_HoaDonChiTiet
AS
SELECT 
    h.MaHD, h. NgayLap, kh.TenNguoiDung AS KhachHang, kh. SDT AS SDT_KhachHang,
    ql.TenNguoiDung AS QuanLy, h.TongTien, h. TrangThai, h.PhuongThucTT
FROM HOADON h
LEFT JOIN NGUOIDUNG kh ON h.MaKH = kh.MaUser
LEFT JOIN NGUOIDUNG ql ON h.MaQL = ql.MaUser;
GO

PRINT N'========== ĐÃ TẠO 2 VIEWS ==========';
GO

/* ============================================================
   PHẦN 8: TẠO INDEX ĐỂ TỐI ƯU
============================================================ */

CREATE NONCLUSTERED INDEX idx_SANPHAM_MaDM ON SANPHAM(MaDM);
CREATE NONCLUSTERED INDEX idx_SANPHAM_MaNCC ON SANPHAM(MaNCC);
CREATE NONCLUSTERED INDEX idx_SANPHAM_TrangThai ON SANPHAM(TrangThai);
CREATE NONCLUSTERED INDEX idx_HOADON_NgayLap ON HOADON(NgayLap);
CREATE NONCLUSTERED INDEX idx_HOADON_TrangThai ON HOADON(TrangThai);
CREATE NONCLUSTERED INDEX idx_NGUOIDUNG_Email ON NGUOIDUNG(Email);
GO

PRINT N'========== ĐÃ TẠO 6 INDEXES ==========';
GO

/* ============================================================
   PHẦN 9: CHƯƠNG 3 - QUẢN TRỊ HỆ THỐNG
   Tạo Login, User, Role và phân quyền
============================================================ */

USE master;
GO

-- Tạo Login cho Admin
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'login_admin')
BEGIN
    CREATE LOGIN login_admin WITH PASSWORD = 'Admin@123', CHECK_POLICY = OFF;
    PRINT N'Đã tạo login_admin';
END
GO

-- Tạo Login cho Quản lý
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'login_quanly')
BEGIN
    CREATE LOGIN login_quanly WITH PASSWORD = 'QuanLy@123', CHECK_POLICY = OFF;
    PRINT N'Đã tạo login_quanly';
END
GO

-- Tạo Login cho Khách hàng
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'login_khach')
BEGIN
    CREATE LOGIN login_khach WITH PASSWORD = 'Khach@123', CHECK_POLICY = OFF;
    PRINT N'Đã tạo login_khach';
END
GO

USE DT_DB;
GO

-- Tạo User từ Login
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'user_admin')
BEGIN
    CREATE USER user_admin FOR LOGIN login_admin;
    PRINT N'Đã tạo user_admin';
END
GO

IF NOT EXISTS (SELECT * FROM sys. database_principals WHERE name = 'user_quanly')
BEGIN
    CREATE USER user_quanly FOR LOGIN login_quanly;
    PRINT N'Đã tạo user_quanly';
END
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'user_khach')
BEGIN
    CREATE USER user_khach FOR LOGIN login_khach;
    PRINT N'Đã tạo user_khach';
END
GO

-- Tạo Role
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'role_admin' AND type = 'R')
BEGIN
    CREATE ROLE role_admin;
    PRINT N'Đã tạo role_admin';
END
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'role_quanly' AND type = 'R')
BEGIN
    CREATE ROLE role_quanly;
    PRINT N'Đã tạo role_quanly';
END
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'role_khach' AND type = 'R')
BEGIN
    CREATE ROLE role_khach;
    PRINT N'Đã tạo role_khach';
END
GO

-- Gán User vào Role
ALTER ROLE role_admin ADD MEMBER user_admin;
ALTER ROLE role_quanly ADD MEMBER user_quanly;
ALTER ROLE role_khach ADD MEMBER user_khach;
GO

-- Phân quyền cho role_admin (toàn quyền)
GRANT CONTROL ON DATABASE::DT_DB TO role_admin;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO role_admin;
GRANT EXECUTE ON SCHEMA::dbo TO role_admin;
GO

-- Phân quyền cho role_quanly (quản lý hàng hóa, đơn hàng)
GRANT SELECT ON SCHEMA::dbo TO role_quanly;
GRANT INSERT, UPDATE ON SANPHAM TO role_quanly;
GRANT INSERT, UPDATE ON HOADON TO role_quanly;
GRANT INSERT, UPDATE ON CTHOADON TO role_quanly;
GRANT INSERT, UPDATE ON HOADONNHAPHANG TO role_quanly;
GRANT INSERT, UPDATE ON PHIEUNHAP TO role_quanly;
GRANT EXECUTE ON sp_NhapHang TO role_quanly;
GRANT EXECUTE ON sp_TaoHoaDonBan TO role_quanly;
GRANT EXECUTE ON sp_BaoCaoDoanhThu TO role_quanly;
GRANT EXECUTE ON sp_BaoCaoTonKho TO role_quanly;
GRANT EXECUTE ON sp_CheckLowStock TO role_quanly;
GO

-- Phân quyền cho role_khach (chỉ xem và mua hàng)
GRANT SELECT ON vw_SanPhamDayDu TO role_khach;
GRANT SELECT ON DANHMUC TO role_khach;
GRANT SELECT, INSERT ON HOADON TO role_khach;
GRANT SELECT, INSERT ON CTHOADON TO role_khach;
GRANT EXECUTE ON sp_TaoHoaDonBan TO role_khach;
GO

PRINT N'========== ĐÃ TẠO LOGIN, USER, ROLE VÀ PHÂN QUYỀN ==========';
GO

/* ============================================================
   PHẦN 10: THIẾT LẬP SAO LƯU TỰ ĐỘNG
   Tạo SQL Server Agent Job để sao lưu định kỳ
============================================================ */

USE msdb;
GO

-- Job sao lưu FULL hàng ngày lúc 2:00 AM
IF EXISTS (SELECT * FROM msdb.dbo.sysjobs WHERE name = 'DT_DB_Daily_Full_Backup')
    EXEC sp_delete_job @job_name = 'DT_DB_Daily_Full_Backup';
GO

EXEC sp_add_job 
    @job_name = 'DT_DB_Daily_Full_Backup',
    @enabled = 1,
    @description = N'Sao lưu FULL database DT_DB hàng ngày';
GO

EXEC sp_add_jobstep 
    @job_name = 'DT_DB_Daily_Full_Backup',
    @step_name = 'Backup Step',
    @subsystem = 'TSQL',
    @database_name = 'DT_DB',
    @command = 'EXEC sp_BackupDatabase @BackupType = ''FULL''';
GO

EXEC sp_add_schedule 
    @schedule_name = 'Daily_2AM',
    @freq_type = 4, -- Daily
    @freq_interval = 1,
    @active_start_time = 020000; -- 2:00 AM
GO

EXEC sp_attach_schedule 
    @job_name = 'DT_DB_Daily_Full_Backup',
    @schedule_name = 'Daily_2AM';
GO

EXEC sp_add_jobserver 
    @job_name = 'DT_DB_Daily_Full_Backup';
GO

-- Job sao lưu DIFFERENTIAL mỗi 6 giờ
IF EXISTS (SELECT * FROM msdb.dbo.sysjobs WHERE name = 'DT_DB_6Hour_Diff_Backup')
    EXEC sp_delete_job @job_name = 'DT_DB_6Hour_Diff_Backup';
GO

EXEC sp_add_job 
    @job_name = 'DT_DB_6Hour_Diff_Backup',
    @enabled = 1,
    @description = N'Sao lưu DIFFERENTIAL mỗi 6 giờ';
GO

EXEC sp_add_jobstep 
    @job_name = 'DT_DB_6Hour_Diff_Backup',
    @step_name = 'Backup Step',
    @subsystem = 'TSQL',
    @database_name = 'DT_DB',
    @command = 'EXEC sp_BackupDatabase @BackupType = ''DIFFERENTIAL''';
GO

EXEC sp_add_schedule 
    @schedule_name = 'Every_6Hours',
    @freq_type = 4, -- Daily
    @freq_interval = 1,
    @freq_subday_type = 8, -- Hours
    @freq_subday_interval = 6;
GO

EXEC sp_attach_schedule 
    @job_name = 'DT_DB_6Hour_Diff_Backup',
    @schedule_name = 'Every_6Hours';
GO

EXEC sp_add_jobserver 
    @job_name = 'DT_DB_6Hour_Diff_Backup';
GO

PRINT N'========== ĐÃ TẠO SQL AGENT JOBS SAO LƯU TỰ ĐỘNG ==========';
GO

/* ============================================================
   PHẦN 11: INSERT DỮ LIỆU MẪU
============================================================ */

USE DT_DB;
GO

-- Thêm người dùng
INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, Email, SDT, DiaChi, Role)
VALUES
(N'Administrator', '123', 'admin@dt.com', NULL, NULL, 'admin'),
(N'Nguyễn Quản Lý', '123', 'ql@dt.com', '0909000001', N'Q1, TP. HCM', 'quanly'),
(N'Nguyễn Khách Hàng', '123', 'khach@dt.com', '0909000002', N'Q1, TP.HCM', 'khach');
GO

-- Thêm danh mục
INSERT INTO DANHMUC (TenDM, MoTa, HinhAnh)
VALUES
(N'iPhone', N'Dòng Apple', 'imagesLTWEB/dm_iphone.jpg'),
(N'Samsung', N'Dòng Samsung', 'imagesLTWEB/dm_samsung.jpg'),
(N'Oppo', N'Dòng Oppo', 'imagesLTWEB/dm_oppo.jpg'),
(N'Vivo', N'Dòng Vivo', 'imagesLTWEB/dm_vivo.jpg'),
(N'Xiaomi', N'Dòng Xiaomi', 'imagesLTWEB/dm_xiaomi.jpg');
GO

-- Thêm nhà cung cấp
INSERT INTO NHACUNGCAP (TenNCC, SDT, DiaChi, Email)
VALUES
(N'Apple Việt Nam', '0901111111', N'TP. HCM', 'apple@vn.com'),
(N'Samsung Việt Nam', '0902222222', N'TP. HCM', 'samsung@vn.com'),
(N'Oppo Việt Nam', '0903333333', N'TP.HCM', 'oppo@vn. com'),
(N'Vivo Việt Nam', '0904444444', N'TP.HCM', 'vivo@vn.com'),
(N'Xiaomi Việt Nam', '0905555555', N'TP.HCM', 'xiaomi@vn.com');
GO

-- Thêm sản phẩm
INSERT INTO SANPHAM (TenSP, DonGia, DonViTinh, SoLuongTon, HinhAnh, MoTa, MaNCC, MaDM)
VALUES
(N'iPhone 14 Pro Max', 25000000, N'Chiếc', 50, 'imagesLTWEB/dienthoaiiphone. jpg', N'iPhone 14 Pro Max cao cấp', 'NCC001', 'DM001'),
(N'iPhone 15 Pro Max', 23000000, N'Chiếc', 60, 'imagesLTWEB/dienthoaiiphone2.jpg', N'iPhone 15 Pro Max mới nhất', 'NCC001', 'DM001'),
(N'Samsung S20 Ultra', 18000000, N'Chiếc', 50, 'imagesLTWEB/dienthoaisamsung.jpg', N'Samsung S20 Ultra', 'NCC002', 'DM002'),
(N'Samsung S21 Ultra', 16000000, N'Chiếc', 60, 'imagesLTWEB/dienthoaisamsung2.jpg', N'Samsung S21 Ultra', 'NCC002', 'DM002'),
(N'Oppo mẫu 1', 10000000, N'Chiếc', 5, 'imagesLTWEB/dienthoaioppo.jpg', N'Oppo mới nhất', 'NCC003', 'DM003'),
(N'Vivo iQOO 12', 11000000, N'Chiếc', 8, 'imagesLTWEB/dienthoaivivo.jpg', N'Vivo cao cấp', 'NCC004', 'DM004'),
(N'Xiaomi Mi 14 Ultra', 9000000, N'Chiếc', 3, 'imagesLTWEB/dienthoaixiaomi.jpg', N'Xiaomi cấu hình cao', 'NCC005', 'DM005');
GO

PRINT N'========== ĐÃ THÊM DỮ LIỆU MẪU ==========';
GO

/* ============================================================
   PHẦN 12: TEST HỆ THỐNG
============================================================ */

PRINT N'';
PRINT N'========================================';
PRINT N'       BẮT ĐẦU TEST HỆ THỐNG';
PRINT N'========================================';
GO

-- Test 1: Function kiểm tra tài khoản
PRINT N'';
PRINT N'[TEST 1] Function fun_check_account:';
SELECT dbo.fun_check_account(N'Administrator', '123') AS [Đăng nhập đúng (phải = 1)];
SELECT dbo.fun_check_account(N'Administrator', 'sai') AS [Đăng nhập sai (phải = 0)];
GO

-- Test 2: Trigger tự sinh mã
PRINT N'';
PRINT N'[TEST 2] Test Trigger tự sinh mã:';
INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, Email, Role)
VALUES (N'Test Trigger User', '123', 'trigger_test@dt.com', 'khach');
SELECT TOP 1 MaUser, TenNguoiDung FROM NGUOIDUNG ORDER BY NgayTao DESC;
GO

-- Test 3: Procedure tạo user với TRANSACTION
PRINT N'';
PRINT N'[TEST 3] Procedure proc_create_user (TRANSACTION):';
EXEC proc_create_user 
    @username = N'Procedure Test User',
    @pass = '123456',
    @email = 'proc_test@dt.com',
    @sdt = '0909999999',
    @role = 'khach';
GO

-- Test 4: Cursor kiểm tra sản phẩm sắp hết
PRINT N'';
PRINT N'[TEST 4] Procedure sp_CheckLowStock (CURSOR):';
EXEC sp_CheckLowStock @SoLuongToiThieu = 10;
GO

-- Test 5: Transaction nhập hàng
PRINT N'';
PRINT N'[TEST 5] Procedure sp_NhapHang (TRANSACTION + TRIGGER):';
EXEC sp_NhapHang 
    @MaNCC = 'NCC001',
    @MaQL = 'US002',
    @ChiTietNhap = '[{"MaSP":"SP001","SoLuong":100,"DonGiaNhap":20000000}]';
GO

-- Test 6: Transaction bán hàng
PRINT N'';
PRINT N'[TEST 6] Procedure sp_TaoHoaDonBan (TRANSACTION + TRIGGER):';
EXEC sp_TaoHoaDonBan 
    @MaKH = 'US003',
    @MaQL = 'US002',
    @PhuongThucTT = N'Tiền mặt',
    @ChiTietBan = '[{"MaSP":"SP001","SoLuong":2,"DonGia":25000000}]';
GO

-- Test 7: Function tính doanh thu
PRINT N'';
PRINT N'[TEST 7] Function fun_get_revenue_by_date:';
SELECT dbo.fun_get_revenue_by_date(CAST(GETDATE() AS DATE)) AS [Doanh thu hôm nay];
GO

-- Test 8: Báo cáo
PRINT N'';
PRINT N'[TEST 8] Procedure sp_BaoCaoTonKho:';
EXEC sp_BaoCaoTonKho;
GO

-- Test 9: View
PRINT N'';
PRINT N'[TEST 9] View vw_SanPhamDayDu:';
SELECT TOP 5 * FROM vw_SanPhamDayDu ORDER BY DonGia DESC;
GO

-- Test 10: Audit Log
PRINT N'';
PRINT N'[TEST 10] Kiểm tra AUDITLOG:';
SELECT TOP 10 * FROM