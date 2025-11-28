namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CTHOADONs = new HashSet<CTHOADON>();
            PHIEUNHAPs = new HashSet<PHIEUNHAP>();
        }

        [Key]
        [StringLength(10)]
        public string MaSP { get; set; }

        [Required]
        [StringLength(100)]
        public string TenSP { get; set; }

        public decimal? DonGia { get; set; }

        [StringLength(20)]
        public string DonViTinh { get; set; }

        public int? SoLuongTon { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        [StringLength(1000)]
        public string MoTa { get; set; }

        [StringLength(10)]
        public string MaNCC { get; set; }

        [StringLength(10)]
        public string MaDM { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHOADON> CTHOADONs { get; set; }

        public virtual DANHMUC DANHMUC { get; set; }

        public virtual NHACUNGCAP NHACUNGCAP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUNHAP> PHIEUNHAPs { get; set; }
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
    }
}
