using ESPL.KP.Entities;
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

namespace ESPL.KP.Services
{
    public interface ILibraryRepository
    {
        #region Authors
        PagedList<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
        void AddAuthor(Author author);
        void DeleteAuthor(Author author);
        void UpdateAuthor(Author author);
        bool AuthorExists(Guid authorId);

        #endregion

        #region Books
        IEnumerable<Book> GetBooksForAuthor(Guid authorId);
        Book GetBookForAuthor(Guid authorId, Guid bookId);
        void AddBookForAuthor(Guid authorId, Book book);
        void UpdateBookForAuthor(Book book);
        void DeleteBook(Book book);

        #endregion


        #region Department
        PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentResourceParameters);
        MstDepartment GetDepartment(Guid departmentId);
        IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds);
        void AddDepartment(MstDepartment department);
        void DeleteDepartment(MstDepartment department);
        void UpdateDepartment(MstDepartment department);
        bool DepartmentExists(Guid authorId);

        #endregion

        #region Area
        PagedList<MstArea> GetAreas(AreasResourceParameters AreaResourceParameters);
        MstArea GetArea(Guid AreaId);
        IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds);
        void AddArea(MstArea Area);
        void DeleteArea(MstArea Area);
        void UpdateArea(MstArea Area);
        bool AreaExists(Guid authorId);

        #endregion

        #region Designation
        PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters DesignationResourceParameters);
        MstDesignation GetDesignation(Guid DesignationId);
        IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> DesignationIds);
        void AddDesignation(MstDesignation Designation);
        void DeleteDesignation(MstDesignation Designation);
        void UpdateDesignation(MstDesignation Designation);
        bool DesignationExists(Guid authorId);

        #endregion

        #region OccurrenceType
        PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters);
        MstOccurrenceType GetOccurrenceType(Guid occurrenceTypeId);
        IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds);
        void AddOccurrenceType(MstOccurrenceType occurrenceType);
        void DeleteOccurrenceType(MstOccurrenceType occurrenceType);
        void UpdateOccurrenceType(MstOccurrenceType occurrenceType);
        bool OccurrenceTypeExists(Guid occurrenceTypeId);
        #endregion

        #region OccurrenceBook
        PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceBookResourceParameters occurrenceTypeResourceParameters);
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

        #endregion

        #region Status
        PagedList<MstStatus> GetStatuses(StatusesResourceParameters statusResourceParameters);
        MstStatus GetStatus(Guid statusId);
        IEnumerable<MstStatus> GetStatuses(IEnumerable<Guid> statusIds);
        void AddStatus(MstStatus status);
        void DeleteStatus(MstStatus status);
        void UpdateStatus(MstStatus status);
        bool StatusExists(Guid authorId);

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

        #endregion AppModule

        #region ESPLUser

        PagedList<ESPLUser> GetESPLUsers(ESPLUsersResourceParameters esplUserResourceParameters);
        ESPLUser GetESPLUser(Guid esplUserId);
        IEnumerable<ESPLUser> GetESPLUsers(IEnumerable<Guid> esplUserIds);
        void AddESPLUser(ESPLUser esplUser);
        void DeleteESPLUser(ESPLUser esplUser);
        void UpdateESPLUser(ESPLUser esplUser);
        bool ESPLUserExists(Guid esplUserId);

        #endregion ESPLUser


         #region ESPLRole

        PagedList<ESPLRole> GetESPLRoles(ESPLRolesResourceParameters esplRoleResourceParameters);
        ESPLRole GetESPLRole(Guid esplRoleId);
        IEnumerable<ESPLRole> GetESPLRoles(IEnumerable<Guid> esplRoleIds);
        void AddESPLRole(ESPLRole esplRole);
        void DeleteESPLRole(ESPLRole esplRole);
        void UpdateESPLRole(ESPLRole esplRole);
        bool ESPLRoleExists(Guid esplRoleId);

        #endregion ESPLRole

        #region Employee
        PagedList<MstEmployee> GetEmployees(EmployeesResourceParameters employeesResourceParameters);
        MstEmployee GetEmployee(Guid employeeId);
        IEnumerable<MstEmployee> GetEmployees(IEnumerable<Guid> employeeIds);
        void AddEmployee(MstEmployee employee);
        void DeleteEmployee(MstEmployee employee);
        void UpdateEmployee(MstEmployee employee);
        bool EmployeeExists(Guid authorId);

        #endregion
    }
}