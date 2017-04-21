using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CfgEmployeeDesignation : BaseEntity {
        [Key]
        public Guid EmployeeDesignationID { get; set; }

        [ForeignKey ("EmployeeID")]
        public MstEmployee MstEmployee { get; set; }
        public Guid EmployeeID { get; set; }

        [ForeignKey ("DesignationID")]
        public MstDesignation MstDesignation { get; set; }
        public Guid DesignationID { get; set; }
    }
}