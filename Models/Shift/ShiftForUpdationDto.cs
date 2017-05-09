using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class ShiftForUpdationDto : BaseDto
    {
        public ShiftForUpdationDto()
        {
        }
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}