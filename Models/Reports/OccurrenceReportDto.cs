using System;
using ESPL.KP.Models.Core;
using  ESPL.KP.Entities;

namespace ESPL.KP.Models
{
    public class OccurrenceReportDto : BaseDto
    {
        //public Guid OBID { get; set; }

        public MstArea Area { get; set; }

        public MstOccurrenceType Offence { get; set; }

        public DepartmentDto Department { get; set; }

        public MstStatus Status { get; set; }

        public string OBNumber { get; set; }

        public DateTime OBTime { get; set; }

        public string CaseFileNumber { get; set; }

        public string NatureOfOccurrence { get; set; }

        public string Remark { get; set; }
    }
}