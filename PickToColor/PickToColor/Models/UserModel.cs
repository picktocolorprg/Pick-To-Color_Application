using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsManager { get; set; }
        public bool IsOperator { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ImagePath { get; set; }
    }
}