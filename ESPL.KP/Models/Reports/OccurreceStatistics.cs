using System;
using ESPL.KP.Models.Core;
using  ESPL.KP.Entities;
using System.Linq;

namespace ESPL.KP.Models
{

    public class StatusStatistics
    {
       
        public int Count { get; set; }

        public string StatusName { get; set; }
    }

    public class PriorityStatistics
    {
       
        public int Count { get; set; }

        public string Priority { get; set; }
    }

  public class OccurrencesStatistics
    {
        public IQueryable<StatusStatistics> StatusWiseStats { get; set; }

        public IQueryable<PriorityStatistics> PriorityWiseStats{ get; set; } 

        public int Total{ get; set; } 
    }

    public class OfficersStatistics
    {
        public IQueryable<StatusStatistics> StatusWiseStats { get; set; }

        public IQueryable<PriorityStatistics> PriorityWiseStats{ get; set; } 

        public int Total{ get; set; } 
    }

     public class Statistics
    {
        public OccurrencesStatistics OccurrencesStatistics { get; set; }

        public OfficersStatistics OfficersStatistics{ get; set; } 

    }

}