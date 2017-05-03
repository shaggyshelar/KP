using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class OccurrenceBookReviewsForCreationDto : BaseDto
    {
        public Guid OBID { get; set; }
        public string ReveiwComments { get; set; }
    }
}