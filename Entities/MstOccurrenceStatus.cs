using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities {
    public class MstOccurrenceStatus : BaseEntity {
        [Key]
        public Guid StatusID { get; set; }

        [Required]
        [MaxLength (50)]
        public string StatusName { get; set; }

        public ICollection<MstOccurrenceBook> MstOccurrenceBooks { get; set; } = new List<MstOccurrenceBook> ();
        public ICollection<OccurrenceAssignment> OccurrenceAssignments { get; set; } = new List<OccurrenceAssignment> ();
    }
}