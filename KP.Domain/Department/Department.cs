using System;
using KP.Domain.Common;

namespace KP.Domain.Department
{
    public class Department : BaseEntity
    {
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentDespcription { get; set; }
    }
}