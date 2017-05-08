using System;
using ESPL.KP.Models.Core;
using ESPL.KP.Entities;
using ESPL.KP.Enums;

namespace ESPL.KP.Models
{
    public class OccurrenceBookActivityDto : BaseDto
    {
        public string OBID { get; set; }
        public DateTime OBTime { get; set; }
        public string NatureOfOccurrence { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}