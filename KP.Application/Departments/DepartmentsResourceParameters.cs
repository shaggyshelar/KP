using KP.Common.Helpers;

namespace KP.Application.Departments
{
    public class DepartmentsResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "DepartmentName";        
    }
}