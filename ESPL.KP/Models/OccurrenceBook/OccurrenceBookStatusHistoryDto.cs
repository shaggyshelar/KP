using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceBookStatusHistoryDto : BaseDto
    {
        public Guid OccurrenceStatusHistoryID { get; set; }
        public Guid OBID { get; set; }
        public Guid StatusID { get; set; }
        public string Comments { get; set; }
    }
}