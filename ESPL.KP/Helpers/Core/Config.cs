using System.Collections.Generic;

namespace ESPL.KP.Helpers.Core
{
    public class Config
    {
        public static IList<string> GetAppModulesList()
        {
            return new List<string>(){
                "DP",
                "DS",
                "AR",
                "OT",
                "ST",
                "SF",
                "EP",
                "OB",
                "RP",
                "DB"
            };
        }
    }
}