﻿using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using System;
using System.Collections.Generic;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.Department;
using ESPL.KP.Helpers.Area;
using ESPL.KP.Helpers.Designation;
using ESPL.KP.Helpers.OccurrenceType;
using ESPL.KP.Helpers.OccurrenceBook;
using ESPL.KP.Helpers.Shift;
using ESPL.KP.Helpers.Status;
using ESPL.KP.Entities.Core;
using ESPL.KP.Helpers.Employee;
using ESPL.KP.Helpers.Reports;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ESPL.KP.Models;
using System.Threading.Tasks;

namespace ESPL.KP.Services
{
    public interface IAppRepository
    {
        #region Department
        PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentResourceParameters);
        MstDepartment GetDepartment(Guid departmentId);
        IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds);
        void AddDepartment(MstDepartment department);
        void DeleteDepartment(MstDepartment department);
        void UpdateDepartment(MstDepartment department);
        bool DepartmentExists(Guid authorId);
        IEnumerable<LookUpItem> GetDepartmentAsLookUp();

        #endregion

        #region Area
        PagedList<MstArea> GetAreas(AreasResourceParameters AreaResourceParameters);
        MstArea GetArea(Guid AreaId);
        IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds);
        void AddArea(MstArea Area);
        void DeleteArea(MstArea Area);
        void UpdateArea(MstArea Area);
        bool AreaExists(Guid authorId);
        IEnumerable<LookUpItem> GetAreaAsLookUp();

        #endregion

        #region Designation
        PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters DesignationResourceParameters);
        MstDesignation GetDesignation(Guid DesignationId);
        IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> DesignationIds);
        void AddDesignation(MstDesignation Designation);
        void DeleteDesignation(MstDesignation Designation);
        void UpdateDesignation(MstDesignation Designation);
        bool DesignationExists(Guid authorId);
        IEnumerable<LookUpItem> GetDesignationAsLookUp();

        #endregion

        #region OccurrenceType
        PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters);
        MstOccurrenceType GetOccurrenceType(Guid occurrenceTypeId);
        IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds);
        void AddOccurrenceType(MstOccurrenceType occurrenceType);
        void DeleteOccurrenceType(MstOccurrenceType occurrenceType);
        void UpdateOccurrenceType(MstOccurrenceType occurrenceType);
        bool OccurrenceTypeExists(Guid occurrenceTypeId);
        IEnumerable<LookUpItem> GetOccurrenceTypeAsLookUp();
        #endregion

        #region OccurrenceBook
        PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceBookResourceParameters occurrenceTypeResourceParameters);
        PagedList<OccurrenceBookActivity> GetOccurrenceBookActivity(OccurrenceBookActivityResourceParameters occurrenceBookActivityResourceParameters);
        MstOccurrenceBook GetOccurrenceBook(Guid occurrenceTypeId);
        IEnumerable<MstOccurrenceBook> GetOccurrenceBooks(IEnumerable<Guid> occurrenceTypeIds);
        void AddOccurrenceBook(MstOccurrenceBook occurrenceType);
        void DeleteOccurrenceBook(MstOccurrenceBook occurrenceType);
        void UpdateOccurrenceBook(MstOccurrenceBook occurrenceType);
        bool OccurrenceBookExists(Guid occurrenceTypeId);
        #endregion

        #region Shift
        PagedList<MstShift> GetShifts(ShiftsResourceParameters shiftResourceParameters);
        MstShift GetShift(Guid shiftId);
        IEnumerable<MstShift> GetShifts(IEnumerable<Guid> shiftIds);
        void AddShift(MstShift shift);
        void DeleteShift(MstShift shift);
        void UpdateShift(MstShift shift);
        bool ShiftExists(Guid authorId);
        IEnumerable<LookUpItem> GetShiftAsLookUp();

        #endregion

        #region Status
        PagedList<MstStatus> GetStatuses(StatusesResourceParameters statusResourceParameters);
        MstStatus GetStatus(Guid statusId);
        IEnumerable<MstStatus> GetStatuses(IEnumerable<Guid> statusIds);
        void AddStatus(MstStatus status);
        void DeleteStatus(MstStatus status);
        void UpdateStatus(MstStatus status);
        bool StatusExists(Guid authorId);
        MstStatus GetStatusByName(string statusName);
        IEnumerable<LookUpItem> GetStatusAsLookUp();
        #endregion 

        bool Save();

        #region AppModule

        PagedList<AppModule> GetAppModules(AppModulesResourceParameters appModuleResourceParameters);
        AppModule GetAppModule(Guid appModuleId);
        IEnumerable<AppModule> GetAppModules(IEnumerable<Guid> appModuleIds);
        void AddAppModule(AppModule appModule);
        void DeleteAppModule(AppModule appModule);
        void UpdateAppModule(AppModule appModule);
        bool AppModuleExists(Guid appModuleId);
        
        bool AppModuleExists(string appModuleName);
        IEnumerable<LookUpItem> GetAppModulesAsLookUp();

        #endregion AppModule

        #region AppUser

        PagedList<AppUser> GetAppUsers(AppUsersResourceParameters esplUserResourceParameters);
        AppUser GetAppUser(Guid esplUserId);
        IEnumerable<AppUser> GetAppUsers(IEnumerable<Guid> esplUserIds);
        void AddAppUser(AppUser esplUser);
        void DeleteAppUser(AppUser esplUser);
        void UpdateAppUser(AppUser esplUser);
        bool AppUserExists(Guid esplUserId);

        #endregion AppUser


        #region AppRole

        PagedList<IdentityRole> GetAppRoles(AppRolesResourceParameters esplRoleResourceParameters);
        IdentityRole GetAppRole(Guid esplRoleId);
        IEnumerable<IdentityRole> GetAppRoles(IEnumerable<Guid> esplRoleIds);
        Task AddAppRole(IdentityRole esplRole);
        Task DeleteAppRole(IdentityRole esplRole);
        void UpdateAppRole(IdentityRole esplRole);
        bool AppRoleExists(Guid esplRoleId);
        IEnumerable<LookUpItem> GetAppRolesAsLookUp();
        #endregion AppRole

        #region Employee
        PagedList<MstEmployee> GetEmployees(EmployeesResourceParameters employeesResourceParameters);
        MstEmployee GetEmployee(Guid employeeId);
        MstEmployee GetEmployeeByUserID(Guid userId);
        IEnumerable<MstEmployee> GetEmployees(IEnumerable<Guid> employeeIds);
        void AddEmployee(MstEmployee employee);
        void DeleteEmployee(MstEmployee employee);
        void UpdateEmployee(MstEmployee employee);
        bool EmployeeExists(Guid authorId);
        IEnumerable<LookUpItem> GetUsersWithoutEmployees();
        IEnumerable<LookUpItem> GetEmployeeAsLookUp();


        #endregion

        #region Reports
        PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceReportResourceParameters occurrenceTypeResourceParameters);
        Statistics GetOccurrenceBooksStatistics(OccurrenceStatisticsResourceParameters occurrenceTypeResourceParameters);
        //OccurreceStatistics GetOfficersStatistics(OccurrenceStatisticsResourceParameters occurrenceTypeResourceParameters);
        #endregion

        #region OccurrenceAssignmentHistory
        void AddOccurrenceAssignmentHistory(OccurrenceAssignmentHistory occurrenceBookhistory);
        PagedList<OccurrenceAssignmentHistory> GetAssignmentHistory(Guid obid, OccurrenceBookAssignedToResourceParameters occurrenceBookAssignedHistory);
        #endregion OccurrenceAssignmentHistory

        #region OccurrenceReviewHistory
        PagedList<OccurrenceReviewHistory> GetOccurrenceReviewHistories(Guid Obid, OccurrenceBookReviewResourceParameters occurrenceBookReviewResourceParameters);
        OccurrenceReviewHistory GetReviewById(Guid occurrenceBookId, Guid reviewId);
        void AddOccurrenceReviewHistories(OccurrenceReviewHistory occurrenceReviewHistory);
        #endregion OccurrenceReviewHistory

        #region Status History
        PagedList<OccurrenceStatusHistory> GetStatusHistory(Guid id, OccurrenceBookStatusResourceParameters occurrenceBookStatusHistory);
        void AddOccurrenceStatusHistory(OccurrenceStatusHistory occurrenceBookStatusHistory);
        #endregion Status History

        #region Employee History
        void AddEmployeeAreaHistory(CfgEmployeeArea employeeAreaHistory);
        void AddEmployeeDepartmentHistory(CfgEmployeeDepartment employeeDepartmentHistory);
        void AddEmployeeDesignationHistory(CfgEmployeeDesignation employeeDesignationHistory);
        void AddEmployeeShiftHistory(CfgEmployeeShift employeeShiftHistory);
        PagedList<CfgEmployeeShift> GetEmployeeShiftHistory(Guid id, EmployeeShiftHistoryResourceParameters employeeStatusHistoryParams);
        PagedList<CfgEmployeeDepartment> GetEmployeeDepartmentHistory(Guid id, EmployeeDepartmentHistoryResourceParameters employeeDepartmentHistoryParams);
        #endregion Employee History
    }
}