using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESPL.KP.Models
{
    public class ShiftForCreationDto
    {
        public string ShiftName { get; set; }
        public TimeSpan? MondayStartTime { get; set; }        
        public TimeSpan? MondayEndTime { get; set; }        
        public TimeSpan? TuesdayStartTime { get; set; }        
        public TimeSpan? TuesdayEndTime { get; set; }
        public TimeSpan? WednesdayStartTime { get; set; }        
        public TimeSpan? WednesdayEndTime { get; set; }
        public TimeSpan? ThursdayStartTime { get; set; }        
        public TimeSpan? ThursdayEndTime { get; set; }
        public TimeSpan? FridayStartTime { get; set; }        
        public TimeSpan? FridayEndTime { get; set; }
        public TimeSpan? SaturdayStartTime { get; set; }        
        public TimeSpan? SaturdayEndTime { get; set; }
        public TimeSpan? SundayStartTime { get; set; }        
        public TimeSpan? SundayEndTime { get; set; }
    }
}
