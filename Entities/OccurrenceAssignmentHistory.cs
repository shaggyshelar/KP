using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class OccurrenceAssignmentHistory : BaseEntity {
        [Key]
        public Guid OBAssignmentID { get; set; }

        [ForeignKey ("OBID")]
        public MstOccurrenceBook MstOccurrenceBook { get; set; }
        public Guid OBID { get; set; }
        
        [ForeignKey ("AssignedTO")]
        public MstEmployee MstEmployee { get; set; }
        public Guid AssignedTO { get; set; }

        //[MaxLength (500)]
        // public string Comments { get; set; }
    }
}