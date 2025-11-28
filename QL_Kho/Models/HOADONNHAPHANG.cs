namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADONNHAPHANG")]
    public partial class HOADONNHAPHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOADONNHAPHANG()
        {
            PHIEUNHAPs = new HashSet<PHIEUNHAP>();
        }

        [Key]
        [StringLength(10)]
        public string MaHDN { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayNhap { get; set; }

        public decimal? TongTien { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [Required]
        [StringLength(10)]
        public string MaQL { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }

        public virtual NHACUNGCAP NHACUNGCAP { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUNHAP> PHIEUNHAPs { get; set; }
    }
}
