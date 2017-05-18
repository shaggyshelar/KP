using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Department
{
    public class DepartmentsResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "DepartmentName";
        private int _pageSize = 0;
        public override int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}