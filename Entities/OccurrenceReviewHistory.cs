using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class OccurrenceReviewHistory : BaseEntity {
        [Key]
        public System.Guid OBReviewHistoryID { get; set; }

        [ForeignKey ("OBID")]
        public virtual MstOccurrenceBook MstOccurrenceBook { get; set; }
        public System.Guid OBID { get; set; }

        [Required]
        [MaxLength (500)]
        public string ReveiwComments { get; set; }

    }
}