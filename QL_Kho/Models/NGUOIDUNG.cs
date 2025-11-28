namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NGUOIDUNG")]
    public partial class NGUOIDUNG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NGUOIDUNG()
        {
            HOADONs = new HashSet<HOADON>();
            HOADONs1 = new HashSet<HOADON>();
            HOADONNHAPHANGs = new HashSet<HOADONNHAPHANG>();
        }

        [Key]
        [StringLength(10)]
        public string MaUser { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNguoiDung { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADONNHAPHANG> HOADONNHAPHANGs { get; set; }
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
    }
}
