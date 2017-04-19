using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class CFGRolePermission : BaseEntity {
        [Key]
        public Guid RolePermissionID { get; set; }

        [ForeignKey ("PermissioinID")]
        public MstPermission MSTPermission { get; set; }
        public Guid PermissioinID { get; set; }

        [ForeignKey ("RoleID")]
        public MstRole MstRole { get; set; }
        public Guid RoleID { get; set; }
        public bool AddAccess { get; set; }
        public bool UpdateAccess { get; set; }
        public bool DeleteAccess { get; set; }

    }
}