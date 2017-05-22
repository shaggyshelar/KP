using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceBookReviewDto : BaseDto
    {
        public Guid OBReviewHistoryID { get; set; }
        public Guid OBID { get; set; }
        public string ReveiwComments { get; set; }
    }
}