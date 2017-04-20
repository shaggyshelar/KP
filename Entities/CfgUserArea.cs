using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CfgUserArea : BaseEntity {
        [Key]
        public Guid UserAreaID { get; set; }

        [ForeignKey ("UserID")]
        public ESPLUser ESPLUser { get; set; }
        public string UserID { get; set; }

        [ForeignKey ("AreaID")]
        public MstArea MstArea { get; set; }
        public Guid AreaID { get; set; }
    }
}