using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.API.Entities {
    public class MstUserProfile : BaseEntity {
        [Key]
        public Guid UserProfileID { get; set; }

        [ForeignKey ("UserID")]
        public MstUser MstUser { get; set; }
        public Guid UserID { get; set; }

        [Required]
        [MaxLength (50)]
        public string FirsstName { get; set; }

        [Required]
        [MaxLength (50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength (50)]
        public string Email { get; set; }

        [MaxLength (20)]
        public string Phone1 { get; set; }

        [MaxLength (20)]
        public string Phone2 { get; set; }

        [MaxLength (500)]
        public string Address1 { get; set; }

        [MaxLength (500)]
        public string Address2 { get; set; }

    }
}