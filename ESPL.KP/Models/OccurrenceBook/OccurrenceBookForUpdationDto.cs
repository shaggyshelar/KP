using System;
using ESPL.KP.Models.Core;
using ESPL.KP.Enums;
namespace ESPL.KP.Models
{
    public class OccurrenceBookForUpdationDto: BaseDto
    {
        public Guid AreaID { get; set; }
        public Guid OBTypeID { get; set; }
        public Guid DepartmentID { get; set; }
        public Guid StatusID { get; set; }
        public string OBNumber { get; set; }
        public DateTime OBTime { get; set; }
        public string CaseFileNumber { get; set; }
        public string NatureOfOccurrence { get; set; }
        public string Remark { get; set; }
        public Guid AssignedTO { get; set; }
        public string AssignedComments { get; set; }
        public int MapZoomLevel { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public OccurrencePriorities Priority { get; set; }
    }
}