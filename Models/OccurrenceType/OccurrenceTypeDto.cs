using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models.OccurrenceType
{
    public class OccurrenceTypeDto:BaseDto
    {
        public Guid OccurrenceTypeID { get; set; }

        public string OccurrenceTypeName { get; set; }
    }
}