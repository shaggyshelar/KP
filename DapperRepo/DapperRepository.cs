using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static System.Data.CommandType;
using System.Data;
using ESPL.KP.Entities;
using ESPL.KP.DapperRepositoryInterfaces;

namespace ESPL.KP.DapperRepo
{
    public class DapperRepository : BaseRepository, IDapperRepository
    {

        public bool AddArea(MstArea area)
        {
            throw new NotImplementedException();
        }

        public bool DeleteArea(string areaId)
        {
            throw new NotImplementedException();
        }

        //public IList<MstArea> GetAllAreas() => SqlMapper.Query<MstArea>(con, "GetAllAreas", commandType: StoredProcedure).ToList();
        public IList<MstArea> GetAllAreas()
        {
            return con.Query<MstArea>("select * from MstArea").ToList();
        }

        public bool UpdateArea(MstArea area)
        {
            throw new NotImplementedException();
        }
    }
}