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
using ESPL.KP.Helpers.Department;
using ESPL.KP.Helpers.Designation;

namespace ESPL.KP.DapperRepo
{
    public class DapperRepository : BaseRepository, IDapperRepository
    {

        #region Area
        //public IList<MstArea> GetAllAreas() => SqlMapper.Query<MstArea>(con, "GetAllAreas", commandType: StoredProcedure).ToList();
        //SELECT * FROM MstDepartment ORDER BY DepartmentName OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
        public PagedList<MstArea> GetAllAreas(AreasResourceParameters areaResourceParameters)
        {
            string query = "select * from MstArea WHERE IsDelete = 0";
            if (!string.IsNullOrEmpty(areaResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = areaResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                query += @"AND (AreaName LIKE'%" + searchQueryForWhereClause + @"%'
	                        OR AreaCode LIKE'%" + searchQueryForWhereClause + "%')";
            }
            if (!string.IsNullOrEmpty(areaResourceParameters.OrderBy))
            {
                query += " ORDER BY " + areaResourceParameters.OrderBy;
            }

            var collectionBeforePaging = con.Query<MstArea>(query).ToList();
            return PagedList<MstArea>.Create(collectionBeforePaging,
                areaResourceParameters.PageNumber,
                areaResourceParameters.PageSize);
        }

        public MstArea GetArea(Guid areaId)
        {
            string query = "select * from MstArea WHERE AreaID=@areaId AND IsDelete=0";
            return this.con.Query<MstArea>(query, new { areaId }).SingleOrDefault();
        }

        public MstArea AddArea(MstArea area)
        {
            area.AreaID = Guid.NewGuid();
            string query = "INSERT INTO MstArea (AreaID,AreaCode,AreaName,CreatedBy,CreatedOn,IsDelete,PinCode,UpdatedBy,UpdatedOn) VALUES (@AreaID,@AreaCode,@AreaName,@CreatedBy,@CreatedOn,@IsDelete,@PinCode,@UpdatedBy,@UpdatedOn)";
            this.con.Query(query, area).SingleOrDefault();
            return area;
        }
        public void UpdateArea(MstArea area)
        {
            string query = @"UPDATE MstArea SET AreaID = @AreaID, AreaCode = @AreaCode, AreaName = @AreaName, IsDelete = @IsDelete, PinCode = @PinCode, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE AreaID=@AreaID";
            this.con.Execute(query, area);
        }
        public void DeleteArea(Guid areaId)
        {
            string query = @"UPDATE MstArea SET IsDelete = 1 WHERE AreaID=@areaId";
            this.con.Execute(query, new { areaId = Convert.ToString(areaId) });
        }

        public bool AreaExists(Guid AreaId)
        {
            string query = "select * from MstArea WHERE AreaID=@AreaId AND IsDelete=0";
            MstArea area = this.con.Query<MstArea>(query, new { AreaId }).SingleOrDefault();
            if (area != null)
                return true;
            return false;
        }

        public IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds)
        {
            string areaIds = string.Empty;
            var lastItem = AreaIds.Last();
            foreach (var item in AreaIds)
            {
                areaIds += Convert.ToString(item);
                if (item != lastItem)
                {
                    areaIds += ",";
                }
            }
            string query = "select * from MstArea WHERE IsDelete=0 AND AreaID IN (@areaIds) ORDER BY AreaName";
            return this.con.Query<MstArea>(query, new { areaIds = "(" + areaIds + ")" }).ToList();
        }
        #endregion Area

        #region Department
        public PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentsResourceParameters)
        {
            string query = "SELECT * FROM MstDepartment WHERE IsDelete=0";
            if (!string.IsNullOrEmpty(departmentsResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = departmentsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                query += @"AND (DepartmentName LIKE'%" + searchQueryForWhereClause + @"%'
                    OR DepartmentDescription LIKE'%" + searchQueryForWhereClause + "%')";
            }
            if (!string.IsNullOrEmpty(departmentsResourceParameters.OrderBy))
            {
                query += " ORDER BY " + departmentsResourceParameters.OrderBy;
            }

            var collectionBeforePaging = con.Query<MstDepartment>(query).ToList();
            return PagedList<MstDepartment>.Create(collectionBeforePaging,
                departmentsResourceParameters.PageNumber,
                departmentsResourceParameters.PageSize);
        }

        public MstDepartment GetDepartment(Guid departmentID)
        {
            string query = "SELECT * FROM MstDepartment WHERE IsDelete=0 AND DepartmentID=@departmentID";
            return this.con.Query<MstDepartment>(query, new { departmentID }).SingleOrDefault();
        }

        public MstDepartment AddDepartment(MstDepartment department)
        {
            department.DepartmentID = Guid.NewGuid();
            string query = "INSERT INTO MstDepartment (DepartmentID,CreatedBy,CreatedOn,DepartmentDespcription,DepartmentName,IsDelete,UpdatedBy,UpdatedOn) VALUES(@DepartmentID,@CreatedBy,@CreatedOn,@DepartmentDespcription,@DepartmentName,@IsDelete,@UpdatedBy,@UpdatedOn)";
            this.con.Query(query, department).SingleOrDefault();
            return department;
        }
        public void UpdateDepartment(MstDepartment department)
        {
            string query = @"UPDATE MstDepartment SET DepartmentID = @DepartmentID,CreatedBy = @CreatedBy,CreatedOn = @CreatedOn,DepartmentDespcription = @DepartmentDespcription,DepartmentName = @DepartmentName,IsDelete = @IsDelete,UpdatedBy = @UpdatedBy,UpdatedOn = @UpdatedOn,WHERE DepartmentID=@DepartmentID";
            this.con.Execute(query, department);
        }
        public void DeleteDepartment(Guid departmentId)
        {
            string query = @"UPDATE MstDepartment SET IsDelete = 1 WHERE DepartmentID=@departmentId";
            this.con.Execute(query, new { areaId = Convert.ToString(departmentId) });
        }
        public bool DepartmentExists(Guid departmentID)
        {
            string query = "SELECT * FROM MstDepartment WHERE IsDelete=0 AND DepartmentID=@departmentID";
            MstDepartment dept = this.con.Query<MstDepartment>(query, new { departmentID }).SingleOrDefault();
            if (dept != null)
                return true;
            return false;
        }

        public IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds)
        {
            string deptIds = string.Empty;
            var lastItem = departmentIds.Last();
            foreach (var item in departmentIds)
            {
                deptIds += Convert.ToString(item);
                if (item != lastItem)
                {
                    deptIds += ",";
                }
            }
            string query = "SELECT * FROM MstDepartment WHERE IsDelete=0 AND DepartmentID IN (@deptIds) ORDER BY DepartmentName";
            return this.con.Query<MstDepartment>(query, new { deptIds = "(" + deptIds + ")" }).ToList();
        }
        #endregion Department

        #region Designation
        public PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters designationsResourceParameters)
        {
            string query = "SELECT * FROM MstDesignation WHERE IsDelete=0";
            if (!string.IsNullOrEmpty(designationsResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = designationsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();
                query += @" AND (DesignationName LIKE '%" + searchQueryForWhereClause + @"%'
            OR DesignationCode LIKE'%" + searchQueryForWhereClause + "%')";
            }
            if (!string.IsNullOrEmpty(designationsResourceParameters.OrderBy))
            {
                query += " ORDER BY " + designationsResourceParameters.OrderBy;
            }

            var collectionBeforePaging = con.Query<MstDesignation>(query).ToList();
            return PagedList<MstDesignation>.Create(collectionBeforePaging,
                designationsResourceParameters.PageNumber,
                designationsResourceParameters.PageSize);
        }

        public MstDesignation GetDesignation(Guid designationID)
        {
            string query = "SELECT * FROM MstDesignation WHERE IsDelete=0 AND DesignationID=@designationID";
            return this.con.Query<MstDesignation>(query, new { designationID }).SingleOrDefault();
        }

        public MstDesignation AddDesignation(MstDesignation designation)
        {
            designation.DesignationID = Guid.NewGuid();
            string query = "INSERT INTO MstDesignation (DesignationID,CreatedBy,CreatedOn,DesignationCode,DesignationName,IsDelete,UpdatedBy,UpdatedOn) VALUES (@DesignationID,@CreatedBy,@CreatedOn,@DesignationCode,@DesignationName,@IsDelete,@UpdatedBy,@UpdatedOn)";
            this.con.Query(query, designation).SingleOrDefault();
            return designation;
        }
        public void UpdateDesignation(MstDesignation designation)
        {
            string query = @"UPDATE MstDesignation SET DesignationID = @DesignationID,CreatedBy = @CreatedBy,CreatedOn = @CreatedOn,DesignationCode = @DesignationCode,DesignationName = @DesignationName,IsDelete = @IsDelete,UpdatedBy = @UpdatedBy,UpdatedOn = @UpdatedOn WHERE DesignationID=@DesignationID";
            this.con.Execute(query, designation);
        }
        public void DeleteDesignation(Guid designationId)
        {
            string query = @"UPDATE MstDesignation SET IsDelete = 1 WHERE DesignationID=@designationId";
            this.con.Execute(query, new { designationId = Convert.ToString(designationId) });
        }
        public bool DesignationExists(Guid designationID)
        {
            string query = "SELECT * FROM MstDesignation WHERE IsDelete=0 AND DesignationID=@designationID";
            MstDesignation desig = this.con.Query<MstDesignation>(query, new { designationID }).SingleOrDefault();
            if (desig != null)
                return true;
            return false;
        }

        public IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> designationIds)
        {
            string desigIds = string.Empty;
            var lastItem = designationIds.Last();
            foreach (var item in designationIds)
            {
                desigIds += Convert.ToString(item);
                if (item != lastItem)
                {
                    desigIds += ",";
                }
            }
            string query = "SELECT * FROM MstDesignation WHERE IsDelete=0 AND DesignationID IN (@desigIds) ORDER BY DesignationName";
            return this.con.Query<MstDesignation>(query, new { desigIds = "(" + desigIds + ")" }).ToList();
        }
        #endregion Designation

    }
}