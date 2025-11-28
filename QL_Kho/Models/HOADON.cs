namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOADON")]
    public partial class HOADON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOADON()
        {
            CTHOADONs = new HashSet<CTHOADON>();
        }

        [Key]
        [StringLength(10)]
        public string MaHD { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayLap { get; set; }

        public decimal? TongTien { get; set; }

        [StringLength(30)]
        public string TrangThai { get; set; }

        [StringLength(20)]
        public string PhuongThucTT { get; set; }

        [StringLength(10)]
        public string MaKH { get; set; }

        [StringLength(10)]
        public string MaQL { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        public DateTime? NgayTao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHOADON> CTHOADONs { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG { get; set; }

        public virtual NGUOIDUNG NGUOIDUNG1 { get; set; }
    }
}
