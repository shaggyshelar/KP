using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class StatusForUpdationDto : BaseDto
    {
        public StatusForUpdationDto()
        {
        }
        public Guid ShiftID { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}