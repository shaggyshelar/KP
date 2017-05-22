using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class DepartmentDto : BaseDto
    {
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentDespcription { get; set; }
    }
}
