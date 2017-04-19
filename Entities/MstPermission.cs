using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstPermission : BaseEntity {
        [Key]
        public Guid PermissionID { get; set; }

        [Required]
        [MaxLength (50)]
        public string PermissionName { get; set; }

        [ForeignKey ("PermissionParentID")]
        public MstPermission MSTPermission2 { get; set; }
        public Guid PermissionParentID { get; set; }
        public int LogicalSequence { get; set; }
        public string FormName { get; set; }

        public ICollection<CFGRolePermission> CFGRolePermissions { get; set; } = new List<CFGRolePermission> ();
        public ICollection<MstPermission> MSTPermission1 { get; set; } = new List<MstPermission> ();
    }
}