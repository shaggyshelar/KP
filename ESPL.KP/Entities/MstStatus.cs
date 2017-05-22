using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstStatus : BaseEntity {
        [Key]
        public Guid StatusID { get; set; }

        [Required]
        [MaxLength (50)]
        public string StatusName { get; set; }
    }
}