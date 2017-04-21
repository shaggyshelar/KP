using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class StatusDto : BaseDto
    {
        public Guid StatusID { get; set; }
        public string StatusName { get; set; }
        
    }
}
