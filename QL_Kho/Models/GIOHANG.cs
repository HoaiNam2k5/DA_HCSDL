using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_Kho.Models
{
    [Table("GIOHANG")]
    public partial class GIOHANG
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaGH { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaUser { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "char")]
        public string MaSP { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        [Column(TypeName = "decimal")]
        public decimal DonGia { get; set; }

        public DateTime? NgayThem { get; set; }

        // Navigation properties
        public virtual NGUOIDUNG NGUOIDUNG { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }
    }
}