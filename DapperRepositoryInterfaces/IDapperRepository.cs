using System;
using System.Collections.Generic;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Area;

namespace ESPL.KP.DapperRepositoryInterfaces
{
    public interface IDapperRepository
    {
        PagedList<MstArea> GetAllAreas(AreasResourceParameters areasResourceParameters);
        MstArea AddArea(MstArea area);
        void UpdateArea(MstArea area);
        void DeleteArea(Guid areaId);
    }
}