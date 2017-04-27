using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class ShiftDto : BaseDto
    {
        public Guid ShiftID { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }       
        public TimeSpan EndTime { get; set; }
    }
}
