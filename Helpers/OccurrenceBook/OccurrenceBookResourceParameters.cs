using System.Collections.Generic;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.OccurrenceBook
{
    public class OccurrenceBookResourceParameters : BaseResourceParameters
    {
        public OccurrenceBookResourceParameters()
        {
            this.OBDates = new List<string>();
            this.StatusIDs = new List<string>();
            this.AreaIDs = new List<string>();
            this.DepartmentIDs = new List<string>();
        }
        public string OrderBy { get; set; } = "OBTime";
        public List<string> OBDates { get; set; }
        public List<string> StatusIDs { get; set; }
        public List<string> AreaIDs { get; set; }
        public List<string> DepartmentIDs { get; set; }
    }
}