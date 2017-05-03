using System.Collections.Generic;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.OccurrenceBook
{
    public class OccurrenceBookResourceParameters : BaseResourceParameters
    {
        
        public string OrderBy { get; set; } = "OBTime";
        public string OBDate { get; set; }
        public string StatusID { get; set; }
        public string AreaID { get; set; }
        public string DepartmentID { get; set; }
    }
}