using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESPL.KP.Entities {
    public class BaseEntity {
        public BaseEntity () {
            this.CreatedOn = DateTime.Now;            
            this.IsDelete = false;
        }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }
}