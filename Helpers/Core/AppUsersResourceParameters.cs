namespace ESPL.KP.Helpers.Core
{
    public class AppUsersResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "FirstName";

        public string RoleName { get; set; }
        
    }
}