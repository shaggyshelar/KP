using ESPL.KP.Entities;
using ESPL.KP.Helpers;
using ESPL.KP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ESPL.KP.Helpers.Core;
using ESPL.KP.Helpers.Department;
using ESPL.KP.Helpers.Area;
using ESPL.KP.Helpers.Designation;
using ESPL.KP.Helpers.OccurrenceType;
using ESPL.KP.Helpers.Shift;
using ESPL.KP.Helpers.Status;
using ESPL.KP.Helpers.OccurrenceBook;
using ESPL.KP.Entities.Core;
using ESPL.KP.Models.Core;
using ESPL.KP.Helpers.Employee;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ESPL.KP.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;
        private IPropertyMappingService _propertyMappingService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<ESPLUser> _userMgr;

        public LibraryRepository(LibraryContext context,
            IPropertyMappingService propertyMappingService,
            UserManager<ESPLUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            _context.Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author.Books.Any())
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = GetAuthor(authorId);
            if (author != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (book.Id == null)
                {
                    book.Id = Guid.NewGuid();
                }
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public Author GetAuthor(Guid authorId)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(
            AuthorsResourceParameters authorsResourceParameters)
        {
            //var collectionBeforePaging = _context.Authors
            //    .OrderBy(a => a.FirstName)
            //    .ThenBy(a => a.LastName).AsQueryable();

            var collectionBeforePaging =
                _context.Authors.ApplySort(authorsResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            if (!string.IsNullOrEmpty(authorsResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = authorsResourceParameters.Genre
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(authorsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = authorsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Author>.Create(collectionBeforePaging,
                authorsResourceParameters.PageNumber,
                authorsResourceParameters.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return _context.Books
              .Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            return _context.Books
                        .Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        #region Department

        public PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstDepartment.ApplySort(departmentResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<DepartmentDto, MstDepartment>());

            if (!string.IsNullOrEmpty(departmentResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = departmentResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.DepartmentDespcription.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstDepartment>.Create(collectionBeforePaging,
                departmentResourceParameters.PageNumber,
                departmentResourceParameters.PageSize);
        }

        public MstDepartment GetDepartment(Guid departmentId)
        {
            return _context.MstDepartment.FirstOrDefault(a => a.DepartmentID == departmentId);
        }

        public IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds)
        {
            return _context.MstDepartment.Where(a => departmentIds.Contains(a.DepartmentID))
                .OrderBy(a => a.DepartmentName)
                .ToList();
        }

        public void AddDepartment(MstDepartment department)
        {
            department.DepartmentID = Guid.NewGuid();
            _context.MstDepartment.Add(department);
        }

        public void DeleteDepartment(MstDepartment department)
        {
            _context.MstDepartment.Remove(department);
        }

        public void UpdateDepartment(MstDepartment department)
        {
            // no code in this implementation
        }

        public bool DepartmentExists(Guid departmentId)
        {
            return _context.MstDepartment.Any(a => a.DepartmentID == departmentId);
        }

        #endregion Department

        #region Area

        public PagedList<MstArea> GetAreas(AreasResourceParameters AreaResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstArea.ApplySort(AreaResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AreaDto, MstArea>());

            if (!string.IsNullOrEmpty(AreaResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = AreaResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.AreaName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.AreaCode.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstArea>.Create(collectionBeforePaging,
                AreaResourceParameters.PageNumber,
                AreaResourceParameters.PageSize);
        }

        public MstArea GetArea(Guid AreaId)
        {
            return _context.MstArea.FirstOrDefault(a => a.AreaID == AreaId);
        }

        public IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds)
        {
            return _context.MstArea.Where(a => AreaIds.Contains(a.AreaID))
                .OrderBy(a => a.AreaName)
                .ToList();
        }

        public void AddArea(MstArea Area)
        {
            Area.AreaID = Guid.NewGuid();
            _context.MstArea.Add(Area);
        }

        public void DeleteArea(MstArea Area)
        {
            _context.MstArea.Remove(Area);
        }

        public void UpdateArea(MstArea Area)
        {
            // no code in this implementation
        }

        public bool AreaExists(Guid AreaId)
        {
            return _context.MstArea.Any(a => a.AreaID == AreaId);
        }

        #endregion Area

        #region Designation

        public PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters DesignationResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstDesignation.ApplySort(DesignationResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<DesignationDto, MstDesignation>());

            if (!string.IsNullOrEmpty(DesignationResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = DesignationResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.DesignationName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.DesignationCode.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstDesignation>.Create(collectionBeforePaging,
                DesignationResourceParameters.PageNumber,
                DesignationResourceParameters.PageSize);
        }

        public MstDesignation GetDesignation(Guid DesignationId)
        {
            return _context.MstDesignation.FirstOrDefault(a => a.DesignationID == DesignationId);
        }

        public IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> DesignationIds)
        {
            return _context.MstDesignation.Where(a => DesignationIds.Contains(a.DesignationID))
                .OrderBy(a => a.DesignationName)
                .ToList();
        }

        public void AddDesignation(MstDesignation Designation)
        {
            Designation.DesignationID = Guid.NewGuid();
            _context.MstDesignation.Add(Designation);
        }

        public void DeleteDesignation(MstDesignation Designation)
        {
            _context.MstDesignation.Remove(Designation);
        }

        public void UpdateDesignation(MstDesignation Designation)
        {
            // no code in this implementation
        }

        public bool DesignationExists(Guid DesignationId)
        {
            return _context.MstDesignation.Any(a => a.DesignationID == DesignationId);
        }

        #endregion Designation

        #region OccurrenceType

        public PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstOccurrenceType.ApplySort(occurrenceTypeResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<OccurrenceTypeDto, MstOccurrenceType>());

            if (!string.IsNullOrEmpty(occurrenceTypeResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceTypeResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.OBTypeName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstOccurrenceType>.Create(collectionBeforePaging,
                occurrenceTypeResourceParameters.PageNumber,
                occurrenceTypeResourceParameters.PageSize);
        }

        public MstOccurrenceType GetOccurrenceType(Guid occurrenceTypeId)
        {
            return _context.MstOccurrenceType.FirstOrDefault(a => a.OBTypeID == occurrenceTypeId);
        }

        public IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds)
        {
            return _context.MstOccurrenceType.Where(a => occurrenceTypeIds.Contains(a.OBTypeID))
                .OrderBy(a => a.OBTypeName)
                .ToList();
        }

        public void AddOccurrenceType(MstOccurrenceType occurrenceType)
        {
            occurrenceType.OBTypeID = Guid.NewGuid();
            _context.MstOccurrenceType.Add(occurrenceType);
        }

        public void DeleteOccurrenceType(MstOccurrenceType occurrenceType)
        {
            _context.MstOccurrenceType.Remove(occurrenceType);
        }

        public void UpdateOccurrenceType(MstOccurrenceType occurrenceType)
        {
            // no code in this implementation
        }

        public bool OccurrenceTypeExists(Guid occurrenceTypeId)
        {
            return _context.MstOccurrenceType.Any(a => a.OBTypeID == occurrenceTypeId);
        }

        #endregion

        #region Shift

        public PagedList<MstShift> GetShifts(ShiftsResourceParameters shiftResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstShift.ApplySort(shiftResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<ShiftDto, MstShift>());

            if (!string.IsNullOrEmpty(shiftResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = shiftResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ShiftName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || Convert.ToString(a.StartTime).ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || Convert.ToString(a.EndTime).ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstShift>.Create(collectionBeforePaging,
                shiftResourceParameters.PageNumber,
                shiftResourceParameters.PageSize);
        }

        public MstShift GetShift(Guid shiftId)
        {
            return _context.MstShift.FirstOrDefault(a => a.ShiftID == shiftId);
        }

        public IEnumerable<MstShift> GetShifts(IEnumerable<Guid> shiftIds)
        {
            return _context.MstShift.Where(a => shiftIds.Contains(a.ShiftID))
                .OrderBy(a => a.ShiftName)
                .ToList();
        }

        public void AddShift(MstShift shift)
        {
            shift.ShiftID = Guid.NewGuid();
            _context.MstShift.Add(shift);
        }

        public void DeleteShift(MstShift shift)
        {
            _context.MstShift.Remove(shift);
        }

        public void UpdateShift(MstShift shift)
        {
            // no code in this implementation
        }

        public bool ShiftExists(Guid shiftId)
        {
            return _context.MstShift.Any(a => a.ShiftID == shiftId);
        }

        #endregion Shift

        #region Status

        public PagedList<MstStatus> GetStatuses(StatusesResourceParameters statusResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstStatus.ApplySort(statusResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<StatusDto, MstStatus>());

            if (!string.IsNullOrEmpty(statusResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = statusResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.StatusName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<MstStatus>.Create(collectionBeforePaging,
                statusResourceParameters.PageNumber,
                statusResourceParameters.PageSize);
        }

        public MstStatus GetStatus(Guid statusId)
        {
            return _context.MstStatus.FirstOrDefault(a => a.StatusID == statusId);
        }

        public IEnumerable<MstStatus> GetStatuses(IEnumerable<Guid> statusIds)
        {
            return _context.MstStatus.Where(a => statusIds.Contains(a.StatusID))
                .OrderBy(a => a.StatusName)
                .ToList();
        }

        public void AddStatus(MstStatus status)
        {
            status.StatusID = Guid.NewGuid();
            _context.MstStatus.Add(status);
        }

        public void DeleteStatus(MstStatus status)
        {
            _context.MstStatus.Remove(status);
        }

        public void UpdateStatus(MstStatus status)
        {
            // no code in this implementation
        }

        public bool StatusExists(Guid statusId)
        {
            return _context.MstStatus.Any(a => a.StatusID == statusId);
        }

        #endregion Status



        #region OccurrenceBook

        public PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceBookResourceParameters occurrenceBookResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstOccurrenceBook.ApplySort(occurrenceBookResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<OccurrenceBookDto, MstOccurrenceBook>());

            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                        a.OBNumber.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                        a.CaseFileNumber.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                        a.NatureOfOccurrence.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                        a.Remark.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<MstOccurrenceBook>.Create(collectionBeforePaging,
                occurrenceBookResourceParameters.PageNumber,
                occurrenceBookResourceParameters.PageSize);
        }

        public MstOccurrenceBook GetOccurrenceBook(Guid occurrenceBookId)
        {
            return _context.MstOccurrenceBook.FirstOrDefault(a => a.OBID == occurrenceBookId);
        }

        public IEnumerable<MstOccurrenceBook> GetOccurrenceBooks(IEnumerable<Guid> occurrenceBookIds)
        {
            return _context.MstOccurrenceBook.Where(a => occurrenceBookIds.Contains(a.OBID))
                .OrderBy(a => a.OBTime)
                .ToList();
        }

        public void AddOccurrenceBook(MstOccurrenceBook occurrenceBook)
        {
            occurrenceBook.OBID = Guid.NewGuid();
            _context.MstOccurrenceBook.Add(occurrenceBook);
        }

        public void DeleteOccurrenceBook(MstOccurrenceBook occurrenceBook)
        {
            _context.MstOccurrenceBook.Remove(occurrenceBook);
        }

        public void UpdateOccurrenceBook(MstOccurrenceBook occurrenceBook)
        {
            // no code in this implementation
        }

        public bool OccurrenceBookExists(Guid occurrenceBookId)
        {
            return _context.MstOccurrenceBook.Any(a => a.OBID == occurrenceBookId);
        }

        #endregion OccurrenceBook

        #region AppModule

        public PagedList<AppModule> GetAppModules(AppModulesResourceParameters appModuleResourceParameters)
        {
            var collectionBeforePaging =
                _context.AppModules.ApplySort(appModuleResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AppModuleDto, AppModule>());

            if (!string.IsNullOrEmpty(appModuleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = appModuleResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MenuText.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<AppModule>.Create(collectionBeforePaging,
                appModuleResourceParameters.PageNumber,
                appModuleResourceParameters.PageSize);
        }

        public AppModule GetAppModule(Guid appModuleId)
        {
            return _context.AppModules.FirstOrDefault(a => a.Id == appModuleId);
        }

        public IEnumerable<AppModule> GetAppModules(IEnumerable<Guid> appModuleIds)
        {
            return _context.AppModules.Where(a => appModuleIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void AddAppModule(AppModule appModule)
        {
            appModule.Id = Guid.NewGuid();
            _context.AppModules.Add(appModule);
        }

        public void DeleteAppModule(AppModule appModule)
        {
            _context.AppModules.Remove(appModule);
        }

        public void UpdateAppModule(AppModule appModule)
        {
            // no code in this implementation
        }

        public bool AppModuleExists(Guid appModuleId)
        {
            return _context.AppModules.Any(a => a.Id == appModuleId);
        }

        #endregion AppModule



        #region ESPLUser

        public PagedList<ESPLUser> GetESPLUsers(ESPLUsersResourceParameters esplUserResourceParameters)
        {
            throw new NotImplementedException();
        }

        public ESPLUser GetESPLUser(Guid esplUserId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ESPLUser> GetESPLUsers(IEnumerable<Guid> esplUserIds)
        {
            throw new NotImplementedException();
        }

        public void AddESPLUser(ESPLUser esplUser)
        {
            throw new NotImplementedException();
        }

        public void DeleteESPLUser(ESPLUser esplUser)
        {
            throw new NotImplementedException();
        }

        public void UpdateESPLUser(ESPLUser esplUser)
        {
            // no code in this implementation
        }

        public bool ESPLUserExists(Guid esplUserId)
        {
            throw new NotImplementedException();
        }

        #endregion ESPLUser


        #region ESPLRole

        public PagedList<IdentityRole> GetESPLRoles(ESPLRolesResourceParameters esplRoleResourceParameters)
        {
            var collectionBeforePaging =
               _roleMgr.Roles.ApplySort(esplRoleResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AppModuleDto, AppModule>());

            if (!string.IsNullOrEmpty(esplRoleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = esplRoleResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<IdentityRole>.Create(collectionBeforePaging,
                esplRoleResourceParameters.PageNumber,
                esplRoleResourceParameters.PageSize);
        }

        public IdentityRole GetESPLRole(Guid esplRoleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityRole> GetESPLRoles(IEnumerable<Guid> esplRoleIds)
        {
            throw new NotImplementedException();
        }

        public void AddESPLRole(IdentityRole esplRole)
        {
            throw new NotImplementedException();
        }

        public void DeleteESPLRole(IdentityRole esplRole)
        {
            throw new NotImplementedException();
        }

        public void UpdateESPLRole(IdentityRole esplRole)
        {
            // no code in this implementation
        }

        public bool ESPLRoleExists(Guid esplRoleId)
        {
            throw new NotImplementedException();
        }

        #endregion ESPLRole

        #region Employee
        public PagedList<MstEmployee> GetEmployees(EmployeesResourceParameters employeesResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstEmployee.ApplySort(employeesResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<EmployeeDto, MstEmployee>());

            if (!string.IsNullOrEmpty(employeesResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = employeesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.EmployeeCode.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || Convert.ToString(a.DateOfBirth).ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Gender.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Mobile.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Email.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstDesignation.DesignationName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstDepartment.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstArea.AreaName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.ESPLUser.UserName.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<MstEmployee>.Create(collectionBeforePaging,
                employeesResourceParameters.PageNumber,
                employeesResourceParameters.PageSize);
        }

        public MstEmployee GetEmployee(Guid employeeId)
        {
            return _context.MstEmployee.FirstOrDefault(a => a.EmployeeID == employeeId);
        }

        public IEnumerable<MstEmployee> GetEmployees(IEnumerable<Guid> employeeIds)
        {
            return _context.MstEmployee.Where(a => employeeIds.Contains(a.EmployeeID))
                .OrderBy(a => a.FirstName)
                .ToList();
        }

        public void AddEmployee(MstEmployee employee)
        {
            employee.EmployeeID = Guid.NewGuid();
            _context.MstEmployee.Add(employee);
        }

        public void DeleteEmployee(MstEmployee employee)
        {
            _context.MstEmployee.Remove(employee);
        }

        public void UpdateEmployee(MstEmployee employee)
        {
            // no code in this implementation
        }

        public bool EmployeeExists(Guid employeeId)
        {
            return _context.MstEmployee.Any(a => a.EmployeeID == employeeId);
        }

        #endregion Employee
    }
}