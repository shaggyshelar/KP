using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities {
    public class MstRole : BaseEntity {
        [Key]
        public Guid RoleID { get; set; }

        [Required]
        [MaxLength (50)]
        public string RoleName { get; set; }

        public ICollection<CFGRolePermission> CFGRolePermissions { get; set; } = new List<CFGRolePermission> ();
        public ICollection<MstUser> MstUsers { get; set; } = new List<MstUser> ();
    }
}