using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {

    public class CfgUserShift: BaseEntity
    {
        [Key]
        public Guid UserShiftID { get; set; }

        [ForeignKey("UserID")]
        public ESPLUser ESPLUser { get; set; }
        public string UserID { get; set; }

        [ForeignKey("ShiftID")]
        public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }
    }
}