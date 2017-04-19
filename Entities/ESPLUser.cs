using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESPL.KP.Entities {
    public class ESPLUser : IdentityUser {
        [Required]
        [MaxLength (50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength (50)]
        public string LastName { get; set; }

        // [ForeignKey ("ShiftID")]
        // public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }

        // [ForeignKey ("AreaID")]
        // public MstArea MstArea { get; set; }
        public Guid AreaID { get; set; }

        public string Mobile { get; set; }

        [MaxLength (500)]
        public string Address1 { get; set; }

        [MaxLength (500)]
        public string Address2 { get; set; }

        public DateTime LastLogin { get; set; }

        [Required]
        public int FailedPasswordAttemptCount { get; set; }

        // public ICollection<CFGUserDepartment> CFGUserDepartments { get; set; } 
        //     = new List<CFGUserDepartment> ();
        // public virtual ICollection<OccurrenceAssignment> OccurrenceAssignments { get; set; } 
        //     = new List<OccurrenceAssignment> ();

    }
}