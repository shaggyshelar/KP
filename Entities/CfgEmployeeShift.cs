using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {

    public class CfgEmployeeShift: BaseEntity
    {
        [Key]
        public Guid EmployeeShiftID { get; set; }

        [ForeignKey ("EmployeeID")]
        public MstEmployee MstEmployee { get; set; }
        public Guid EmployeeID { get; set; }

        [ForeignKey("ShiftID")]
        public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }
    }
}