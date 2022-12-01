using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PickToColor.Models
{
    [Table("Locations")]
    public class LocationModel
    {
        [Key]
        public int LocationMasterID { get; set; }
        [Required]
        [MaxLength(50)]
        public string LocationID { get; set; }
        [Required]
        [MaxLength(10)]
        public string LocationCode { get; set; }
        [Required]
        [MaxLength(260)]
        public string LocationSoundFile { get; set; }
        [Required]
        [MaxLength(20)]
        public string BackgroundColorCode { get; set; }
        [Required]
        [MaxLength(20)]
        public string FontColorCode { get; set; }
        [Required]
        [MaxLength(260)]
        public string ColorSoundFile { get; set; }

        [Required]       
        public int PickingPriority { get; set; }

        [Required]
        public int StationID { get; set; }      

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public string StationName;
    }
}