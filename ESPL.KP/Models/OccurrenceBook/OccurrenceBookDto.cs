using System;
using ESPL.KP.Models.Core;
using ESPL.KP.Entities;
using ESPL.KP.Enums;

namespace ESPL.KP.Models
{
    public class OccurrenceBookDto : BaseDto
    {
        public Guid OBID { get; set; }

        public Guid AreaID { get; set; }

        public Guid OBTypeID { get; set; }

        public Guid DepartmentID { get; set; }


        public Guid StatusID { get; set; }

        public string OBNumber { get; set; }

        public DateTime OBTime { get; set; }

        public string CaseFileNumber { get; set; }

        public string NatureOfOccurrence { get; set; }

        public string Remark { get; set; }

        public Guid? AssignedTO { get; set; }
        public string AssignedComments { get; set; }

        public int MapZoomLevel { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public DateTime? AssignedTime { get; set; }

        public MstArea MstArea { get; set; }
        public MstDepartment MstDepartment { get; set; }

        public MstOccurrenceType MstOccurrenceType { get; set; }

        public MstStatus MstStatus { get; set; }

        public MstEmployee MstEmployee { get; set; }
        
        public string Priority { get; set; }  
    }
}