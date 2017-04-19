using System;
using System.ComponentModel.DataAnnotations;

namespace ESPL.KP.Entities
{
    public class MstDepartment
    {

        [Key]
        public Guid DepartmentID { get; set; }

        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; }

        [MaxLength(500)]
        public string DepartmentDespcription { get; set; }

        // public ICollection<CFGUserDepartment> CFGUserDepartments { get; set; } = new List<CFGUserDepartment> ();
        // public ICollection<MstOccurrenceBook> MstOccurrenceBooks { get; set; } = new List<MstOccurrenceBook> ();
    }
}