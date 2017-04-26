using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class MstDepartment : BaseEntity {

        [Key]
        public Guid DepartmentID { get; set; }

        [Required]
        [MaxLength (50)]
        public string DepartmentName { get; set; }

        [MaxLength (500)]
        public string DepartmentDespcription { get; set; }

        // public ICollection<MstEmployee> MstEmployees { get; set; } = new List<MstEmployee> ();
        // public ICollection<MstOccurrenceBook> MstOccurrenceBooks { get; set; } = new List<MstOccurrenceBook> ();
    }
}