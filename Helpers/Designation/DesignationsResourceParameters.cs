using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Designation
{
    public class DesignationsResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "DesignationName";
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