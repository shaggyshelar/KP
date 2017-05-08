using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities 
{
    [NotMapped]
    public class OccurrenceBookActivity:BaseEntity
    {
        public string OBID { get; set; }
        public string NatureOfOccurrence { get; set; }      
        public string Type { get; set; }
        public string Value { get; set; }
        public string CreatedByName { get; set; }
    }
}