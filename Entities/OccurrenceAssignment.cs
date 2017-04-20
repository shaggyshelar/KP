using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class OccurrenceAssignment : BaseEntity {
        [Key]
        public Guid OBAssignmentID { get; set; }

        [ForeignKey ("OBID")]
        public MstOccurrenceBook MstOccurrenceBook { get; set; }
        public Guid OBID { get; set; }

        // [ForeignKey ("OBStatusID")]
        // public MstOccurrenceStatus MstOccurrenceStatus { get; set; }
        // public Guid OBStatusID { get; set; }

        [ForeignKey ("AssignedTO")]
        public ESPLUser ESPLUser { get; set; }
        public string AssignedTO { get; set; }

        [MaxLength (500)]
        public string Comments { get; set; }
    }
}