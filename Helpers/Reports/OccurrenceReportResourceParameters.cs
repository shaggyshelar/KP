using ESPL.KP.Helpers.Core;

namespace ESPL.KP.Helpers.Reports
{
    public class OccurrenceReportResourceParameters:BaseResourceParameters
    {
         public string OrderBy { get; set; } = "OBTime";
    }

    public class OccurrenceStatisticsResourceParameters:BaseResourceParameters
    {
         public string OrderBy { get; set; } = "StatusName";
    }
   
   
    public class OfficersStatisticsResourceParameters:BaseResourceParameters
    {
         public string OrderBy { get; set; } = "StatusName";
    }
   
}