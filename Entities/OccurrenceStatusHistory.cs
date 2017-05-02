using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities
{
    public class OccurrenceStatusHistory : BaseEntity 
    {
        [Key]
        public Guid OccurrenceStatusHistoryID { get; set; }

        [ForeignKey ("OBID")]
        public virtual MstOccurrenceBook MstOccurrenceBook { get; set; }
        public Guid OBID { get; set; }

        [ForeignKey("StatusID")]
        public MstStatus MstStatus { get; set; }
        public Guid StatusID { get; set; }

        [MaxLength(500)]
        public string Comments { get; set; }

        
    }
}
