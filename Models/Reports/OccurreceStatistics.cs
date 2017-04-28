using System;
using ESPL.KP.Models.Core;
using  ESPL.KP.Entities;
using System.Linq;

namespace ESPL.KP.Models
{
    public class OccurreceStatistics : BaseDto
    {
       
        public IQueryable<StatusStatistics> StatusWiseStats { get; set; }

        public IQueryable<PriorityStatistics> PriorityWiseStats{ get; set; } 

        public int TotalOccurrences{ get; set; } 

        //  public StatusStatistics StatusWiseStats { get; set; }

        // public PriorityStatistics PriorityWiseStats{ get; set; } 
    }

    public class StatusStatistics : BaseDto
    {
       
        public int Count { get; set; }

        public string StatusName { get; set; }
    }

    public class PriorityStatistics : BaseDto
    {
       
        public int Count { get; set; }

        public string Priority { get; set; }
    }

}