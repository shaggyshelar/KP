using System;

namespace ESPL.KP.Models
{
    public class OccurrenceTypeForCreationDto
    {
        public Guid OBTypeID { get; set; }

        public string OBTypeName { get; set; }

        public bool IsDelete { get; set; }
    }
}