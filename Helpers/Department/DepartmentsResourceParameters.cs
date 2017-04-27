using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Department
{
    public class DepartmentsResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "DepartmentName";
    }
}