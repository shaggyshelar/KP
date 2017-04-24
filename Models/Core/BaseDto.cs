using System;

namespace ESPL.KP.Models.Core
{
    public class BaseDto
    {
        public BaseDto()
        {
            this.CreatedOn = DateTime.Now;
            this.IsDelete = false;
        }

        public DateTime? CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
    }
}