using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstDesignation : BaseEntity {
        [Key]
        public Guid DesignationID { get; set; }

        [Required]
        [MaxLength (50)]
        public string DesignationName { get; set; }

        [MaxLength (20)]
        public string DesignationCode { get; set; }
        

        public ICollection<MstUser> MstUsers { get; set; } = new List<MstUser> ();
    }
}