using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities {
    public class OccurrenceAssignment : BaseEntity {
        [Key]
        public System.Guid OBAssignmentID { get; set; }

        [ForeignKey ("OBID")]
        public MstOccurrenceBook MstOccurrenceBook { get; set; }
        public System.Guid OBID { get; set; }

        [ForeignKey ("OBStatusID")]
        public MstOccurrenceStatus MstOccurrenceStatu { get; set; }
        public System.Guid OBStatusID { get; set; }

        [ForeignKey ("AssignedTO")]
        public MstUser MstUser { get; set; }
        public System.Guid AssignedTO { get; set; }

        [MaxLength (500)]
        public string Comments { get; set; }
    }
}