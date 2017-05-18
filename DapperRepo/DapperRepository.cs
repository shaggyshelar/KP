using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static System.Data.CommandType;
using System.Data;
using ESPL.KP.Entities;
using ESPL.KP.DapperRepositoryInterfaces;
using ESPL.KP.Helpers.Area;
using ESPL.KP.Helpers;

namespace ESPL.KP.DapperRepo
{
    public class DapperRepository : BaseRepository, IDapperRepository
    {

        public MstArea AddArea(MstArea area)
        {
            area.AreaID = Guid.NewGuid();
            string query = "INSERT INTO MstArea (AreaID,AreaCode,AreaName,CreatedBy,CreatedOn,IsDelete,PinCode,UpdatedBy,UpdatedOn) VALUES (@AreaID,@AreaCode,@AreaName,@CreatedBy,@CreatedOn,@IsDelete,@PinCode,@UpdatedBy,@UpdatedOn)";
            this.con.Query(query, area).SingleOrDefault();
            return area;
        }

        public void DeleteArea(Guid areaId)
        {
            string query = @"UPDATE MstArea SET IsDelete = 1 WHERE AreaID=@areaId";
            this.con.Execute(query, new { areaId = Convert.ToString(areaId) });
            // string query = "DELETE FROM MstArea WHERE AreaID = @Id";
            // this.con.Execute(query, new { Id = areaId });
        }

        //public IList<MstArea> GetAllAreas() => SqlMapper.Query<MstArea>(con, "GetAllAreas", commandType: StoredProcedure).ToList();
        public PagedList<MstArea> GetAllAreas(AreasResourceParameters areaResourceParameters)
        {
            string query = "select * from MstArea";
            if (!string.IsNullOrEmpty(areaResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = areaResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                query += @" WHERE AreaName LIKE'%" + searchQueryForWhereClause + @"%'
	                        OR AreaCode LIKE'%" + searchQueryForWhereClause + "%'";
            }
            if (!string.IsNullOrEmpty(areaResourceParameters.OrderBy))
            {
                query += " ORDER BY " + areaResourceParameters.OrderBy;
            }

            var collectionBeforePaging = con.Query<MstArea>(query).ToList();
            return PagedList<MstArea>.Create(collectionBeforePaging,
                areaResourceParameters.PageNumber,
                areaResourceParameters.PageSize);

            //return con.Query<MstArea>("select * from MstArea").ToList();
        }

        public void UpdateArea(MstArea area)
        {
            string query = @"UPDATE MstArea SET AreaID = @AreaID, AreaCode = @AreaCode, AreaName = @AreaName, IsDelete = @IsDelete, PinCode = @PinCode, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE AreaID=@AreaID";
            this.con.Execute(query, area);
        }
    }
}