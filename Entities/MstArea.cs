using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstArea : BaseEntity {
        [Key]
        public Guid AreaID { get; set; }

        [Required]
        [MaxLength (50)]
        public string AreaName { get; set; }

        [MaxLength (20)]
        public string AreaCode { get; set; }

        [MaxLength (20)]
        public string PinCode { get; set; }

        public ICollection<MstOccurrenceBook> MstOccurrenceBooks { get; set; } 
            = new List<MstOccurrenceBook> ();
        public ICollection<MstUser> MstUsers { get; set; }
            = new List<MstUser>();
    }
}