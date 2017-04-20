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

        public string Mobile { get; set; }

        [MaxLength (500)]
        public string Address1 { get; set; }

        [MaxLength (500)]
        public string Address2 { get; set; }

        public DateTime LastLogin { get; set; }

        [Required]
        public int FailedPasswordAttemptCount { get; set; }

        public ICollection<CfgUserDepartment> CfgUserDepartments { get; set; } 
            = new List<CfgUserDepartment> ();
        public virtual ICollection<OccurrenceAssignment> OccurrenceAssignments { get; set; } 
            = new List<OccurrenceAssignment> ();
         public ICollection<CfgUserShift> CfgUserShift { get; set; } 
            = new List<CfgUserShift> ();
        public ICollection<CfgUserArea> CfgUserArea { get; set; } 
            = new List<CfgUserArea> ();
        public ICollection<CfgUserDesignation> CfgUserDesignation { get; set; } 
            = new List<CfgUserDesignation> ();

    }
}