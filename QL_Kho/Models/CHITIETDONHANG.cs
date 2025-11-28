using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_Kho.Models
{
    [Table("CHITIETDONHANG")]
    public partial class CHITIETDONHANG
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaCTDH { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaDH { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaSP { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal DonGia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal")]
        public decimal? ThanhTien { get; set; }

        // Navigation properties
        public virtual DONHANG DONHANG { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}