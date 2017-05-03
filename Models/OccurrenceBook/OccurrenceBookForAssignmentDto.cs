using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceBookForAssignmentDto : BaseDto
    {
        public Guid OBAssignmentID { get; set; }
        public Guid AssignedTO { get; set; }
    }
}