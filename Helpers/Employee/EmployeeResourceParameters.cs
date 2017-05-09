using System.Collections.Generic;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Employee
{
    public class EmployeesResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "FirstName";
        public string DesignationID { get; set; }
        public string AreaID { get; set; }
        public string DepartmentID { get; set; }
        public bool? CaseAssigned { get; set; }=null;
    }
}