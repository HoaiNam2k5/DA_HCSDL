namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PHIEUNHAP")]
    public partial class PHIEUNHAP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaHDN { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaSP { get; set; }

        public int SoLuong { get; set; }

        public decimal DonGiaNhap { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? ThanhTien { get; set; }

        public virtual HOADONNHAPHANG HOADONNHAPHANG { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
