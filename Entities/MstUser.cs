using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstUser : BaseEntity {
        [Key]
        public Guid UserID { get; set; }

        [ForeignKey ("RoleID")]
        public MstRole MstRole { get; set; }
        public Guid RoleID { get; set; }

        [ForeignKey ("DesignationID")]
        public MstDesignation MstDesignation { get; set; }
        public Guid DesignationID { get; set; }

        [ForeignKey ("ShiftID")]
        public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }

        [ForeignKey ("AreaID")]
        public MstArea MstArea { get; set; }
        public Guid AreaID { get; set; }

        [Required]
        [MaxLength (50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength (128)]
        public string Password { get; set; }

        [Required]
        public int PasswrodFormat { get; set; }

        [Required]
        [MaxLength (128)]
        public string PasswrodSalt { get; set; }
        public DateTime LastLogin { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsLockOut { get; set; }

        [Required]
        [MaxLength (256)]
        public string PasswordQuestion { get; set; }

        [Required]
        [MaxLength (128)]
        public string PasswrodAnswer { get; set; }
        public DateTime LastLockoutDate { get; set; }

        [Required]
        public int FailedPasswordAttemptCount { get; set; }

        public ICollection<CFGUserDepartment> CFGUserDepartments { get; set; } = new List<CFGUserDepartment> ();
        public virtual ICollection<MstUserProfile> MstUserProfiles { get; set; } = new List<MstUserProfile> ();
        public virtual ICollection<OccurrenceAssignment> OccurrenceAssignments { get; set; } = new List<OccurrenceAssignment> ();
    }
}