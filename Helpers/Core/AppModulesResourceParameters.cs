namespace ESPL.KP.Helpers.Core
{
    public class AppModulesResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "Name";

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