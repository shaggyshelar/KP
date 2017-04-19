using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CFGUserDepartment : BaseEntity {
        [Key]
        public Guid UserDepartmentID { get; set; }

        [ForeignKey ("UserID")]
        public ESPLUser ESPLUser { get; set; }
        public string UserID { get; set; }

        [ForeignKey ("DepartmentID")]
        public MstDepartment MstDepartment { get; set; }
        public Guid DepartmentID { get; set; }


    }
}