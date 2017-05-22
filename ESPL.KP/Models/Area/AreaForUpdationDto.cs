using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class AreaForUpdationDto : BaseDto
    {
        public AreaForUpdationDto() 
        {
        }

        public string AreaName { get; set; }
        public string AreaCode { get; set; }
        public string PinCode { get; set; }
    }
}