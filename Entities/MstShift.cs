using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstShift : BaseEntity {
        [Key]
        public Guid ShiftID { get; set; }

        [Required]
        [MaxLength (50)]
        public string ShiftName { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }

        public ICollection<ESPLUser> ESPLUser { get; set; }
            = new List<ESPLUser>();
    }
}