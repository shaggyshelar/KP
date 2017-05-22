using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CfgEmployeeDepartment : BaseEntity {
        [Key]
        public Guid EmployeeDepartmentID { get; set; }

        [ForeignKey ("EmployeeID")]
        public MstEmployee MstEmployee { get; set; }
        public Guid EmployeeID { get; set; }

        [ForeignKey ("DepartmentID")]
        public MstDepartment MstDepartment { get; set; }
        public Guid DepartmentID { get; set; }


    }
}