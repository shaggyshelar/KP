using System.Collections.Generic;
using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Employee
{
    public class EmployeesResourceParameters : BaseResourceParameters
    {
         public  EmployeesResourceParameters()
         {
            this.DesignationIDs = new List<string>();
            this.AreaIDs = new List<string>();
            this.DepartmentIDs = new List<string>();
         }
        public string OrderBy { get; set; } = "FirstName";
        public List<string> DesignationIDs { get; set; }
        public List<string> AreaIDs { get; set; }
        public List<string> DepartmentIDs { get; set; }
        public bool? CaseAssigned { get; set; }=null;
    }
}