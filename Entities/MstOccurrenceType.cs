using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstOccurrenceType : BaseEntity {
        [Key]
        public Guid OBTypeID { get; set; }

        [Required]
        [MaxLength (50)]
        public string OBTypeName { get; set; }


        public ICollection<MstOccurrenceBook> MstOccurrenceBooks { get; set; } = new List<MstOccurrenceBook> ();
    }
}