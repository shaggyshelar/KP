using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CfgUserDesignation : BaseEntity {
        [Key]
        public Guid UserDesignationID { get; set; }

        [ForeignKey ("UserID")]
        public ESPLUser ESPLUser { get; set; }
        public string UserID { get; set; }

        [ForeignKey ("DesignationID")]
        public MstDesignation MstDesignation { get; set; }
        public Guid DesignationID { get; set; }
    }
}