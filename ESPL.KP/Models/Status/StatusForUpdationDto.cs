using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class StatusForUpdationDto : BaseDto
    {
        public StatusForUpdationDto()
        {
        }
        public string StatusName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}