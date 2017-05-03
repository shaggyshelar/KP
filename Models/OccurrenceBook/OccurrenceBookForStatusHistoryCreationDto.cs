using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceBookForStatusHistoryCreationDto : BaseDto
    {
        public Guid OBID { get; set; }
        public Guid StatusID { get; set; }
        public string Comments { get; set; }
    }
}