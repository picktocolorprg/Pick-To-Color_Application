using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Customers")]
    public class CustomerModel
    {
        [Key]
        public int CustomerID { get; set; }
        [Required]
        [MaxLength(20)]
        public string CustomerCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string CustomerName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public bool IsEnabledOneScanning { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}