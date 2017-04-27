using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CfgEmployeeArea : BaseEntity {
        [Key]
        public Guid EmployeeAreaID { get; set; }

        [ForeignKey ("EmployeeID")]
        public MstEmployee MstEmployee { get; set; }
        public Guid EmployeeID { get; set; }

        [ForeignKey ("AreaID")]
        public MstArea MstArea { get; set; }
        public Guid AreaID { get; set; }
    }
}