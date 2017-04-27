using System;
using ESPL.KP.Models.Core;
using  ESPL.KP.Entities;

namespace ESPL.KP.Models
{
    public class OccurreceStatistics : BaseDto
    {
       
        public int Count { get; set; }

        public string StatusName { get; set; }
    }
}