using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceTypeDto:BaseDto
    {
        public Guid OBTypeID { get; set; }

        public string OBTypeName { get; set; }
    }
}