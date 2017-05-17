using System.Collections.Generic;
using ESPL.KP.Entities;

namespace ESPL.KP.DapperRepositoryInterfaces
{
    public interface IDapperRepository
    {
        IList<MstArea> GetAllAreas();
        bool AddArea(MstArea area);
        bool UpdateArea(MstArea area);
        bool DeleteArea(string areaId);
    }
}