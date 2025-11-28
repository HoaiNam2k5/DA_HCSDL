namespace QL_Kho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BACKUP_HISTORY
    {
        [Key]
        public int BackupID { get; set; }

        [StringLength(20)]
        public string BackupType { get; set; }

        [StringLength(500)]
        public string BackupPath { get; set; }

        public long? BackupSize { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
