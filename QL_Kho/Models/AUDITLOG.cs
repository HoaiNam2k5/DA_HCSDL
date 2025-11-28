namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AUDITLOG")]
    public partial class AUDITLOG
    {
        [Key]
        public int LogID { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(20)]
        public string Operation { get; set; }

        [StringLength(10)]
        public string MaUser { get; set; }

        public DateTime? NgayThucHien { get; set; }

        public string NoiDung { get; set; }
    }
}
