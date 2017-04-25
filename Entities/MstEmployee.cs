using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESPL.KP.Entities
{
    public class MstEmployee : BaseEntity
    {
        [Key]
        public Guid EmployeeID { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }       
        
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        
        [MaxLength(20)]
        public string ResidencePhone { get; set; }

        [Required]
        public DateTime OrganizationJoiningDate { get; set; }

        [Required]
        public DateTime ServiceJoiningDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address1 { get; set; }

        [MaxLength(500)]
        public string Address2 { get; set; }

        [ForeignKey ("AreaID")]
        public MstArea MstArea { get; set; }
        public Guid AreaID { get; set; }

        [ForeignKey ("DepartmentID")]
        public MstDepartment MstDepartment { get; set; }
        public Guid DepartmentID { get; set; }

        [ForeignKey ("DesignationID")]
        public MstDesignation MstDesignation { get; set; }
        public Guid DesignationID { get; set; }

        [ForeignKey("ShiftID")]
        public MstShift MstShift { get; set; }
        public Guid ShiftID { get; set; }

        [ForeignKey ("UserID")]
        public ESPLUser ESPLUser { get; set; }
        public string UserID { get; set; }


         public ICollection<CfgEmployeeDepartment> CfgEmployeeDepartments { get; set; } 
            = new List<CfgEmployeeDepartment> ();
        public virtual ICollection<OccurrenceAssignmentHistory> OccurrenceAssignments { get; set; } 
            = new List<OccurrenceAssignmentHistory> ();
         public ICollection<CfgEmployeeShift> CfgEmployeeShift { get; set; } 
            = new List<CfgEmployeeShift> ();
        public ICollection<CfgEmployeeArea> CfgEmployeeArea { get; set; } 
            = new List<CfgEmployeeArea> ();
        public ICollection<CfgEmployeeDesignation> CfgEmployeeDesignation { get; set; } 
            = new List<CfgEmployeeDesignation> ();
    }
}