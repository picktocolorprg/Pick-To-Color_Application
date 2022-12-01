using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("TransactionAudit")]
    public class TransactionAuditModel
    {
        [Key]
        public int AuditID { get; set; }
        public int AuditType { get; set; }
        public int AuditTypeID { get; set; }
        public int UserID { get; set; }
        public string TransactionDescription { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}