using System;
using ESPL.KP.Entities;

namespace ESPL.KP.Models
{
    public class EmployeeShiftHistoryDto
    {
        public Guid EmployeeShiftID { get; set; }
        public MstEmployee MstEmployee { get; set; }
        public Guid EmployeeID { get; set; }
        public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }

    }
}