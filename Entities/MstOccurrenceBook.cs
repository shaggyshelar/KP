using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESPL.KP.Enums;

namespace ESPL.KP.Entities
{
    public class MstOccurrenceBook : BaseEntity
    {
        [Key]
        public Guid OBID { get; set; }

        [ForeignKey("AreaID")]
        public MstArea MstArea { get; set; }
        public Guid? AreaID { get; set; }

        [ForeignKey("OBTypeID")]
        public MstOccurrenceType MstOccurrenceType { get; set; }
        public Guid OBTypeID { get; set; }

        [ForeignKey("DepartmentID")]
        public MstDepartment MstDepartment { get; set; }
        public Guid? DepartmentID { get; set; }

        [ForeignKey("StatusID")]
        public MstStatus MstStatus { get; set; }
        public Guid StatusID { get; set; }

        [Required]
        [MaxLength(50)]
        public string OBNumber { get; set; }

        [Required]
        public DateTime OBTime { get; set; }

        [MaxLength(50)]
        public string CaseFileNumber { get; set; }

        [Required]
        [MaxLength (1000)]
        public string NatureOfOccurrence { get; set; }

        [MaxLength(50)]
        public string Remark { get; set; }

        [ForeignKey("AssignedTO")]
        public MstEmployee MstEmployee { get; set; }
        public Guid? AssignedTO { get; set; }

        [MaxLength(500)]
        public string AssignedComments { get; set; }        
        public DateTime? AssignedTime {get; set;}
        public OccurrencePriorities? Priority { get; set; }
    }
}