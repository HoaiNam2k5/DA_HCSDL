using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_Kho.Models
{
    [Table("DONHANG")]
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft. Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            CHITIETDONHANGs = new HashSet<CHITIETDONHANG>();
        }

        [Key]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaDH { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaUser { get; set; }

        public DateTime NgayDat { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TongTien { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [StringLength(200)]
        public string DiaChiGiao { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar")]
        public string SDT { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [StringLength(30)]
        public string PhuongThucTT { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        // Navigation properties
        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }
    }
}