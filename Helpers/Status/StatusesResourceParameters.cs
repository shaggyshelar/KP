using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Status
{
    public class StatusesResourceParameters : BaseResourceParameters
    {
        public string OrderBy { get; set; } = "StatusName";
    }
}