using System;

namespace ESPL.KP.Models
{
    public class EmployeeDepartmentHistoryDto
    {
        public Guid EmployeeDepartmentID { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid DepartmentID { get; set; }


    }
}