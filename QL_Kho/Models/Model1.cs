using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QL_Kho.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<AUDITLOG> AUDITLOGs { get; set; }
        public virtual DbSet<BACKUP_HISTORY> BACKUP_HISTORY { get; set; }
        public virtual DbSet<CTHOADON> CTHOADONs { get; set; }
        public virtual DbSet<DANHMUC> DANHMUCs { get; set; }
        public virtual DbSet<HOADON> HOADONs { get; set; }
        public virtual DbSet<HOADONNHAPHANG> HOADONNHAPHANGs { get; set; }
        public virtual DbSet<NGUOIDUNG> NGUOIDUNGs { get; set; }
        public virtual DbSet<NHACUNGCAP> NHACUNGCAPs { get; set; }
        public virtual DbSet<PHIEUNHAP> PHIEUNHAPs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }

        // ✅ 3 DbSet mới
        public virtual DbSet<GIOHANG> GIOHANGs { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<CHITIETDONHANG> CHITIETDONHANGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ========== CẤU HÌNH CŨ ==========
            modelBuilder.Entity<AUDITLOG>()
                .Property(e => e.MaUser)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTHOADON>()
                .Property(e => e.MaHD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTHOADON>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CTHOADON>()
                .Property(e => e.ThanhTien)
                .HasPrecision(29, 2);

            modelBuilder.Entity<DANHMUC>()
                .Property(e => e.MaDM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DANHMUC>()
                .Property(e => e.TrangThai)
                .IsUnicode(false);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.MaHD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.MaKH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADON>()
                .Property(e => e.MaQL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADONNHAPHANG>()
                .Property(e => e.MaHDN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADONNHAPHANG>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADONNHAPHANG>()
                .Property(e => e.MaQL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HOADONNHAPHANG>()
                .Property(e => e.TrangThai)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.MaUser)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .Property(e => e.TrangThai)
                .IsUnicode(false);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.HOADONs)
                .WithOptional(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.MaKH);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.HOADONs1)
                .WithOptional(e => e.NGUOIDUNG1)
                .HasForeignKey(e => e.MaQL);

            modelBuilder.Entity<NGUOIDUNG>()
                .HasMany(e => e.HOADONNHAPHANGs)
                .WithRequired(e => e.NGUOIDUNG)
                .HasForeignKey(e => e.MaQL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<NHACUNGCAP>()
                .Property(e => e.TrangThai)
                .IsUnicode(false);

            modelBuilder.Entity<NHACUNGCAP>()
                .HasMany(e => e.HOADONNHAPHANGs)
                .WithRequired(e => e.NHACUNGCAP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.MaHDN)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PHIEUNHAP>()
                .Property(e => e.ThanhTien)
                .HasPrecision(29, 2);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MaNCC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MaDM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.TrangThai)
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .HasMany(e => e.CTHOADONs)
                .WithRequired(e => e.SANPHAM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SANPHAM>()
                .HasMany(e => e.PHIEUNHAPs)
                .WithRequired(e => e.SANPHAM)
                .WillCascadeOnDelete(false);



            // === GIOHANG ===
            modelBuilder.Entity<GIOHANG>()
                .ToTable("GIOHANG");

            modelBuilder.Entity<GIOHANG>()
                .Property(g => g.MaUser)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GIOHANG>()
                .Property(g => g.MaSP)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GIOHANG>()
                .Property(g => g.DonGia)
                .HasPrecision(18, 2);

            // === DONHANG ===
            modelBuilder.Entity<DONHANG>()
                .ToTable("DONHANG");

            modelBuilder.Entity<DONHANG>()
                .Property(d => d.MaDH)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .Property(d => d.MaUser)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DONHANG>()
                .Property(d => d.TongTien)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DONHANG>()
                .Property(d => d.SDT)
                .HasMaxLength(15)
                .IsUnicode(false);

            // === CHITIETDONHANG ===
            modelBuilder.Entity<CHITIETDONHANG>()
                .ToTable("CHITIETDONHANG");

            modelBuilder.Entity<CHITIETDONHANG>()
                .Property(c => c.MaDH)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CHITIETDONHANG>()
                .Property(c => c.MaSP)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CHITIETDONHANG>()
                .Property(c => c.DonGia)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CHITIETDONHANG>()
                .Property(c => c.ThanhTien)
                .HasPrecision(18, 2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}