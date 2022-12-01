using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Boxes")]
    public class BoxesModel
    {
        [Key]
        public int BoxTypeID { get; set; }
        [Required]
        [MaxLength(20)]
        public string BoxTypeCode { get; set; }
        [Required]
        [MaxLength(50)]
        public string BoxTypeDescription { get; set; }
        [Required]
        public decimal TotalCBM { get; set; }
        [Required]
        public Int32 CustomerID { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}