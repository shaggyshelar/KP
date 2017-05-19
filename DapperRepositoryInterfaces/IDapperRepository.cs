using System;
using System.Collections.Generic;
using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Helpers.Area;
using ESPL.KP.Helpers.Department;
using ESPL.KP.Helpers.Designation;

namespace ESPL.KP.DapperRepositoryInterfaces
{
    public interface IDapperRepository
    {
        #region area
        PagedList<MstArea> GetAllAreas(AreasResourceParameters areasResourceParameters);
        MstArea GetArea(Guid areaId);
        IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds);
        MstArea AddArea(MstArea area);
        void UpdateArea(MstArea area);
        void DeleteArea(Guid areaId);
        bool AreaExists(Guid AreaId);
        #endregion area

        #region department
        PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentsResourceParameters);
        MstDepartment GetDepartment(Guid departmentID);
        IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds);
        MstDepartment AddDepartment(MstDepartment department);
        void UpdateDepartment(MstDepartment department);
        void DeleteDepartment(Guid departmentId);
        bool DepartmentExists(Guid departmentID);
        #endregion department

        #region Designation
        PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters designationsResourceParameters);
        MstDesignation GetDesignation(Guid designationID);
        MstDesignation AddDesignation(MstDesignation designation);
        void UpdateDesignation(MstDesignation designation);
        void DeleteDesignation(Guid designationId);
        bool DesignationExists(Guid designationID);
        IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> designationIds);
        #endregion Designation
    }
}