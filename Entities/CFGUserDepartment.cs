using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CFGUserDepartment {
        [Key]
        public Guid UserDepartmentID { get; set; }
        public Guid UserID { get; set; }
        public Guid DepartmentID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }

        [ForeignKey ("DepartmentID")]
        public MstDepartment MstDepartment { get; set; }
        public MstUser MstUser { get; set; }
    }
}