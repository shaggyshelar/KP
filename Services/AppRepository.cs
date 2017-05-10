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
using ESPL.KP.Helpers.Reports;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ESPL.KP.Services
{
    public class AppRepository : IAppRepository
    {
        private Entities.ApplicationContext _context;
        private IPropertyMappingService _propertyMappingService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<AppUser> _userMgr;

        public AppRepository(Entities.ApplicationContext context,
            IPropertyMappingService propertyMappingService,
            UserManager<AppUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        #region Department

        public PagedList<MstDepartment> GetDepartments(DepartmentsResourceParameters departmentResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstDepartment.Where(a => a.IsDelete == false).ApplySort(departmentResourceParameters.OrderBy,
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
            return _context.MstDepartment.FirstOrDefault(a => a.DepartmentID == departmentId && a.IsDelete == false);
        }

        public IEnumerable<MstDepartment> GetDepartments(IEnumerable<Guid> departmentIds)
        {
            return _context.MstDepartment.Where(a => departmentIds.Contains(a.DepartmentID) && a.IsDelete == false)
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
            return _context.MstDepartment.Any(a => a.DepartmentID == departmentId && a.IsDelete == false);
        }

        #endregion Department

        #region Area

        public PagedList<MstArea> GetAreas(AreasResourceParameters AreaResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstArea.Where(a => a.IsDelete == false).ApplySort(AreaResourceParameters.OrderBy,
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
            return _context.MstArea.FirstOrDefault(a => a.AreaID == AreaId && a.IsDelete == false);
        }

        public IEnumerable<MstArea> GetAreas(IEnumerable<Guid> AreaIds)
        {
            return _context.MstArea.Where(a => AreaIds.Contains(a.AreaID) && a.IsDelete == false)
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
            return _context.MstArea.Any(a => a.AreaID == AreaId && a.IsDelete == false);
        }

        #endregion Area

        #region Designation

        public PagedList<MstDesignation> GetDesignations(DesignationsResourceParameters DesignationResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstDesignation.Where(a => a.IsDelete == false).ApplySort(DesignationResourceParameters.OrderBy,
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
            return _context.MstDesignation.FirstOrDefault(a => a.DesignationID == DesignationId && a.IsDelete == false);
        }

        public IEnumerable<MstDesignation> GetDesignations(IEnumerable<Guid> DesignationIds)
        {
            return _context.MstDesignation.Where(a => DesignationIds.Contains(a.DesignationID) && a.IsDelete == false)
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
            return _context.MstDesignation.Any(a => a.DesignationID == DesignationId && a.IsDelete == false);
        }

        #endregion Designation

        #region OccurrenceType

        public PagedList<MstOccurrenceType> GetOccurrenceTypes(OccurrenceTypeResourceParameters occurrenceTypeResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstOccurrenceType.Where(a => a.IsDelete == false).ApplySort(occurrenceTypeResourceParameters.OrderBy,
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
            return _context.MstOccurrenceType.FirstOrDefault(a => a.OBTypeID == occurrenceTypeId && a.IsDelete == false);
        }

        public IEnumerable<MstOccurrenceType> GetOccurrenceType(IEnumerable<Guid> occurrenceTypeIds)
        {
            return _context.MstOccurrenceType.Where(a => occurrenceTypeIds.Contains(a.OBTypeID) && a.IsDelete == false)
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
            return _context.MstOccurrenceType.Any(a => a.OBTypeID == occurrenceTypeId && a.IsDelete == false);
        }

        #endregion

        #region Shift

        public PagedList<MstShift> GetShifts(ShiftsResourceParameters shiftResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstShift.Where(a => a.IsDelete == false).ApplySort(shiftResourceParameters.OrderBy,
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
            return _context.MstShift.FirstOrDefault(a => a.ShiftID == shiftId && a.IsDelete == false);
        }

        public IEnumerable<MstShift> GetShifts(IEnumerable<Guid> shiftIds)
        {
            return _context.MstShift.Where(a => shiftIds.Contains(a.ShiftID) && a.IsDelete == false)
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
            return _context.MstShift.Any(a => a.ShiftID == shiftId && a.IsDelete == false);
        }

        #endregion Shift

        #region Status

        public PagedList<MstStatus> GetStatuses(StatusesResourceParameters statusResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstStatus.Where(a => a.IsDelete == false).ApplySort(statusResourceParameters.OrderBy,
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
            return _context.MstStatus.FirstOrDefault(a => a.StatusID == statusId && a.IsDelete == false);
        }

        public IEnumerable<MstStatus> GetStatuses(IEnumerable<Guid> statusIds)
        {
            return _context.MstStatus.Where(a => statusIds.Contains(a.StatusID) && a.IsDelete == false)
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
            return _context.MstStatus.Any(a => a.StatusID == statusId && a.IsDelete == false);
        }

        public MstStatus GetStatusByName(string statusName)
        {
            return _context.MstStatus.FirstOrDefault(a => a.StatusName == statusName && a.IsDelete == false);
        }

        #endregion Status



        #region OccurrenceBook

        public PagedList<OccurrenceBookActivity> GetOccurrenceBookActivity(OccurrenceBookActivityResourceParameters occurrenceBookActivityResourceParameters)
        {
            var obActivity = (from o in _context.MstOccurrenceBook
                              join os in _context.OccurrenceStatusHistory on o.OBID equals os.OBID
                              join s in _context.MstStatus on os.StatusID equals s.StatusID
                              join e in _context.MstEmployee on os.CreatedBy.Value equals e.EmployeeID
                              select new OccurrenceBookActivity
                              {
                                  OBID = o.OBID.ToString(),
                                  OBNumber = o.OBNumber,
                                  NatureOfOccurrence = o.NatureOfOccurrence,
                                  CreatedOn = os.CreatedOn,
                                  Type = "Status",
                                  Value = s.StatusName,
                                  CreatedByName = e.FirstName + " " + e.LastName
                              })
                          .Union((from o in _context.MstOccurrenceBook
                                  join oc in _context.OccurrenceReviewHistory on o.OBID equals oc.OBID
                                  join e in _context.MstEmployee on oc.CreatedBy.Value equals e.EmployeeID
                                  select new OccurrenceBookActivity
                                  {
                                      OBID = o.OBID.ToString(),
                                      OBNumber = o.OBNumber,
                                      NatureOfOccurrence = o.NatureOfOccurrence,
                                      CreatedOn = oc.CreatedOn,
                                      Type = "Comments",
                                      Value = oc.ReveiwComments,
                                      CreatedByName = e.FirstName + " " + e.LastName
                                  }))
                          .Union((from o in _context.MstOccurrenceBook
                                  join os in _context.OccurrenceAssignmentHistory on o.OBID equals os.OBID
                                  join e in _context.MstEmployee on os.CreatedBy.Value equals e.EmployeeID
                                  join a in _context.MstEmployee on o.AssignedTO equals a.EmployeeID
                                  select new OccurrenceBookActivity
                                  {
                                      OBID = o.OBID.ToString(),
                                      OBNumber = o.OBNumber,
                                      NatureOfOccurrence = o.NatureOfOccurrence,
                                      CreatedOn = os.CreatedOn,
                                      Type = "AssignedTo",
                                      Value = a.FirstName + " " + a.LastName,
                                      CreatedByName = e.FirstName + " " + e.LastName
                                  }));
            obActivity = obActivity.ApplySort(occurrenceBookActivityResourceParameters.OrderBy,
               _propertyMappingService.GetPropertyMapping<OccurrenceBookActivityDto, OccurrenceBookActivity>());

            //Filter Logic
            if (!string.IsNullOrEmpty(occurrenceBookActivityResourceParameters.OBID))
            {
                var obIDs = occurrenceBookActivityResourceParameters.OBID.Split(',');
                obActivity = obActivity
                    .Where(o => obIDs.Contains(o.OBID.ToString()));
            }

            if (!string.IsNullOrEmpty(occurrenceBookActivityResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookActivityResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                obActivity = obActivity
                            .Where(a =>
                                a.OBID.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.OBNumber.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.NatureOfOccurrence.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.Type.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.Value.ToLowerInvariant().Contains(searchQueryForWhereClause)
                                || a.CreatedByName.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<OccurrenceBookActivity>.Create(obActivity, occurrenceBookActivityResourceParameters.PageNumber,
               occurrenceBookActivityResourceParameters.PageSize);
        }



        public PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceBookResourceParameters occurrenceBookResourceParameters)
        {
            var collectionBeforePaging =
               _context.MstOccurrenceBook.Where(a => a.IsDelete == false)
               .Include(ob => ob.MstArea)
               .Include(ob => ob.MstDepartment)
               .Include(ob => ob.MstOccurrenceType)
               .Include(ob => ob.MstStatus)
               .Include(ob => ob.MstEmployee).ThenInclude(e => e.AppUser)
               .ApplySort(occurrenceBookResourceParameters.OrderBy,
               _propertyMappingService.GetPropertyMapping<OccurrenceBookDto, MstOccurrenceBook>());

            //Filter Implementation
            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.AreaID))
            {
                var areaIds = occurrenceBookResourceParameters.AreaID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => areaIds.Contains(o.AreaID.ToString().ToLowerInvariant()));
            }
            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.DepartmentID))
            {
                var departmentIDs = occurrenceBookResourceParameters.DepartmentID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => departmentIDs.Contains(o.DepartmentID.ToString().ToLowerInvariant()));
            }
            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.StatusID))
            {
                var statusIDs = occurrenceBookResourceParameters.StatusID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => statusIDs.Contains(o.StatusID.ToString().ToLowerInvariant()));
            }
            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.OBDate))
            {
                var oBDates = occurrenceBookResourceParameters.OBDate.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => oBDates.Contains(o.OBTime.ToString("MM/dd/yyyy")));
            }
            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.AssignedTo))
            {
                var assignedToIDs = occurrenceBookResourceParameters.AssignedTo.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => assignedToIDs.Contains(o.AssignedTO.ToString().ToLowerInvariant()));
            }
            //Filter Ends


            if (!string.IsNullOrEmpty(occurrenceBookResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                        a.OBNumber.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.CaseFileNumber.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.NatureOfOccurrence.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.Remark.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstOccurrenceType.OBTypeName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstDepartment.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstArea.AreaName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstStatus.StatusName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstEmployee.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        || a.MstEmployee.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<MstOccurrenceBook>.Create(collectionBeforePaging,
                occurrenceBookResourceParameters.PageNumber,
                occurrenceBookResourceParameters.PageSize);
        }

        public MstOccurrenceBook GetOccurrenceBook(Guid occurrenceBookId)
        {
            return _context.MstOccurrenceBook
                .Where(a => a.IsDelete == false)
                .Include(ob => ob.MstArea)
                .Include(ob => ob.MstDepartment)
                .Include(ob => ob.MstOccurrenceType)
                .Include(ob => ob.MstStatus)
                .Include(ob => ob.MstEmployee).ThenInclude(e => e.AppUser)
                .FirstOrDefault(a => a.OBID == occurrenceBookId);
        }

        public IEnumerable<MstOccurrenceBook> GetOccurrenceBooks(IEnumerable<Guid> occurrenceBookIds)
        {
            return _context.MstOccurrenceBook
                .Where(a => a.IsDelete == false)
                .Include(ob => ob.MstArea)
                .Include(ob => ob.MstDepartment)
                .Include(ob => ob.MstOccurrenceType)
                .Include(ob => ob.MstStatus)
                .Include(ob => ob.MstEmployee).ThenInclude(e => e.AppUser)
                .Where(a => occurrenceBookIds.Contains(a.OBID))
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
            return _context.MstOccurrenceBook.Any(a => a.OBID == occurrenceBookId && a.IsDelete == false);
        }

        public void UpdateOccurrenceBookAssignment(MstOccurrenceBook occurrenceBook)
        {
            // no code in this implementation
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

        public bool AppModuleExists(string appModuleName)
        {
            return _context.AppModules.Any(a => a.Name == appModuleName);
        }

        #endregion AppModule



        #region AppUser

        public PagedList<AppUser> GetAppUsers(AppUsersResourceParameters esplUserResourceParameters)
        {
            var collectionBeforePaging =
               _userMgr.Users.ApplySort(esplUserResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<AppUserDto, AppUser>());

            if (!string.IsNullOrEmpty(esplUserResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = esplUserResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Email.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<AppUser>.Create(collectionBeforePaging,
                esplUserResourceParameters.PageNumber,
                esplUserResourceParameters.PageSize);
        }

        public AppUser GetAppUser(Guid esplUserId)
        {
            return _userMgr.Users.FirstOrDefault(a => a.Id == esplUserId.ToString());
        }

        public IEnumerable<AppUser> GetAppUsers(IEnumerable<Guid> esplUserIds)
        {
            return _userMgr.Users.Where(a => esplUserIds.Contains(new Guid(a.Id)))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void AddAppUser(AppUser esplUser)
        {
            _userMgr.CreateAsync(esplUser);
        }

        public async void DeleteAppUser(AppUser esplUser)
        {
            await _userMgr.DeleteAsync(esplUser);
        }

        public void UpdateAppUser(AppUser esplUser)
        {
            // no code in this implementation
        }

        public bool AppUserExists(Guid esplUserId)
        {
            return _userMgr.Users.Any(a => a.Id == esplUserId.ToString());
        }

        #endregion AppUser


        #region AppRole

        public PagedList<IdentityRole> GetAppRoles(AppRolesResourceParameters esplRoleResourceParameters)
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

        public IdentityRole GetAppRole(Guid esplRoleId)
        {
            return _roleMgr.Roles.FirstOrDefault(a => a.Id == esplRoleId.ToString());
        }

        public IEnumerable<IdentityRole> GetAppRoles(IEnumerable<Guid> esplRoleIds)
        {
            return _roleMgr.Roles.Where(a => esplRoleIds.Contains(new Guid(a.Id)))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void AddAppRole(IdentityRole esplRole)
        {
            _roleMgr.CreateAsync(esplRole);
        }

        public async void DeleteAppRole(IdentityRole esplRole)
        {
            await _roleMgr.DeleteAsync(esplRole);
        }

        public void UpdateAppRole(IdentityRole esplRole)
        {
            // no code in this implementation
        }

        public bool AppRoleExists(Guid esplRoleId)
        {
            return _roleMgr.Roles.Any(a => a.Id == esplRoleId.ToString());
        }

        #endregion AppRole

        #region Employee
        public PagedList<MstEmployee> GetEmployees(EmployeesResourceParameters employeesResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstEmployee
                .Where(a => a.IsDelete == false)
                .Include(e => e.MstArea)
                .Include(e => e.MstDepartment)
                .Include(e => e.MstDesignation)
                .Include(e => e.MstShift)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstStatus)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstOccurrenceType)
                .Include(e => e.AppUser)
                .ApplySort(employeesResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<EmployeeDto, MstEmployee>());

            //Filter Implementation
            if (!string.IsNullOrEmpty(employeesResourceParameters.AreaID))
            {
                var areaIds = employeesResourceParameters.AreaID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => areaIds.Contains(o.AreaID.ToString().ToLowerInvariant()));
            }
            if (!string.IsNullOrEmpty(employeesResourceParameters.DepartmentID))
            {
                var departmentIDs = employeesResourceParameters.DepartmentID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => departmentIDs.Contains(o.DepartmentID.ToString().ToLowerInvariant()));
            }
            if (!string.IsNullOrEmpty(employeesResourceParameters.DesignationID))
            {
                var designationIDs = employeesResourceParameters.DesignationID.Split(',');
                collectionBeforePaging = collectionBeforePaging
                    .Where(o => designationIDs.Contains(o.DesignationID.ToString().ToLowerInvariant()));
            }
            if (employeesResourceParameters.CaseAssigned != null)
            {
                var caseAssigned = employeesResourceParameters.CaseAssigned.Value;
                collectionBeforePaging = caseAssigned ? collectionBeforePaging
                    .Where(e => e.MstOccurrenceBooks.Count > 0) : collectionBeforePaging
                    .Where(e => e.MstOccurrenceBooks.Count == 0);
            }
            //Filter Ends

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
                    || a.AppUser.UserName.ToLowerInvariant().Contains(searchQueryForWhereClause));

            }

            return PagedList<MstEmployee>.Create(collectionBeforePaging,
                employeesResourceParameters.PageNumber,
                employeesResourceParameters.PageSize);
        }

        public MstEmployee GetEmployee(Guid employeeId)
        {
            return _context.MstEmployee
                .Where(a => a.IsDelete == false)
                .Include(e => e.MstArea)
                .Include(e => e.MstDepartment)
                .Include(e => e.MstDesignation)
                .Include(e => e.MstShift)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstStatus)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstOccurrenceType)
                .Include(e => e.AppUser)
                .FirstOrDefault(a => a.EmployeeID == employeeId);
        }

        public MstEmployee GetEmployeeByUserID(Guid userId)
        {
            return _context.MstEmployee
                .Where(a => a.IsDelete == false)
                .Include(e => e.MstArea)
                .Include(e => e.MstDepartment)
                .Include(e => e.MstDesignation)
                .Include(e => e.MstShift)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstStatus)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstOccurrenceType)
                .Include(e => e.AppUser)
                .FirstOrDefault(a => a.UserID == userId.ToString());
        }


        public IEnumerable<MstEmployee> GetEmployees(IEnumerable<Guid> employeeIds)
        {
            return _context.MstEmployee
                .Where(a => a.IsDelete == false)
                .Include(e => e.MstArea)
                .Include(e => e.MstDepartment)
                .Include(e => e.MstDesignation)
                .Include(e => e.MstShift)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstStatus)
                .Include(e => e.MstOccurrenceBooks).ThenInclude(ob => ob.MstOccurrenceType)
                .Include(e => e.AppUser)
                .Where(a => employeeIds.Contains(a.EmployeeID))
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
            return _context.MstEmployee.Any(a => a.EmployeeID == employeeId && a.IsDelete == false);
        }

        #endregion Employee

        #region Reports
        public PagedList<MstOccurrenceBook> GetOccurrenceBooks(OccurrenceReportResourceParameters occurrenceBookResourceParameters)
        {
            var collectionBeforePaging =
                _context.MstOccurrenceBook
                .Where(a => a.IsDelete == false)
                .Include(occurrence => occurrence.MstArea)
                .Include(occurrence => occurrence.MstDepartment)
                .Include(occurrence => occurrence.MstStatus)
                .ApplySort(occurrenceBookResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<OccurrenceReportDto, MstOccurrenceBook>());

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

        public Statistics GetOccurrenceBooksStatistics(OccurrenceStatisticsResourceParameters occurrenceBookResourceParameters)
        {
            #region Occurrence Stats
            IQueryable<StatusStatistics> OccurrenceStatusStats = from p in _context.MstOccurrenceBook
                                                                 where p.IsDelete == false
                                                                 group p by p.MstStatus.StatusName into g
                                                                 select new StatusStatistics
                                                                 {
                                                                     StatusName = g.Key,
                                                                     Count = g.Count()
                                                                 };
            IQueryable<PriorityStatistics> OccurrencePriorityStats = from p in _context.MstOccurrenceBook
                                                                     where p.IsDelete == false
                                                                     group p by p.Priority into g
                                                                     select new PriorityStatistics
                                                                     {
                                                                         Priority = g.Key.ToString(),
                                                                         Count = g.Count(),
                                                                     };
            int OccurrenceCount = _context.MstOccurrenceBook.Where(a => a.IsDelete == false).Count();
            #endregion Occurrence Stats

            #region Officers Stats
            IQueryable<StatusStatistics> OfficersStatusStats = from p in _context.MstOccurrenceBook
                                                               where p.IsDelete == false
                                                               where p.AssignedTO != null
                                                               group p by p.MstStatus.StatusName into g
                                                               select new StatusStatistics
                                                               {
                                                                   StatusName = g.Key,
                                                                   Count = g.Count()
                                                               };
            IQueryable<PriorityStatistics> OfficersPriorityStats = from p in _context.MstOccurrenceBook
                                                                   where p.IsDelete == false
                                                                   where p.AssignedTO != null
                                                                   //group p by new { Priority = p.Priority } into g
                                                                   group p by p.Priority into g
                                                                   select new PriorityStatistics
                                                                   {
                                                                       Priority = g.Key.ToString(),
                                                                       Count = g.Count(),
                                                                   };
            int OfficersCount = _context.MstEmployee.Where(a => a.IsDelete == false).Count();
            #endregion Officers Stats



            OccurrencesStatistics OccurrencesStats = new OccurrencesStatistics();
            OfficersStatistics OfficersStats = new OfficersStatistics();

            OccurrencesStats.StatusWiseStats = OccurrenceStatusStats;
            OccurrencesStats.PriorityWiseStats = OccurrencePriorityStats;
            OccurrencesStats.Total = OccurrenceCount;

            OfficersStats.StatusWiseStats = OfficersStatusStats;
            OfficersStats.PriorityWiseStats = OfficersPriorityStats;
            OfficersStats.Total = OfficersCount;

            Statistics collectionBeforePaging = new Statistics();
            collectionBeforePaging.OccurrencesStatistics = OccurrencesStats;
            collectionBeforePaging.OfficersStatistics = OfficersStats;
            return collectionBeforePaging;
        }

        // public Statistics GetOfficersStatistics(OccurrenceStatisticsResourceParameters occurrenceBookResourceParameters)
        // {


        //     Statistics collectionBeforePaging = new Statistics();
        //     collectionBeforePaging.OccurrencesStatistics.StatusWiseStats = statusStats;
        //     collectionBeforePaging.PriorityWiseStats = priorityStats;
        //     collectionBeforePaging.TotalOccurrences = count;
        //     return collectionBeforePaging;
        // }
        #endregion Reports

        #region OccurrenceAssignmentHistory
        public void AddOccurrenceAssignmentHistory(OccurrenceAssignmentHistory occurrenceBookhistory)
        {
            occurrenceBookhistory.OBAssignmentID = Guid.NewGuid();
            _context.OccurrenceAssignmentHistory.Add(occurrenceBookhistory);
        }

        public PagedList<OccurrenceAssignmentHistory> GetAssignmentHistory(Guid obid, OccurrenceBookAssignedToResourceParameters occurrenceBookAssignedHistory)
        {
            var collectionBeforePaging =
                            _context.OccurrenceAssignmentHistory.Where(a => a.OBID == obid)
                            .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
                            .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
                            .Select(assignmnt => new OccurrenceAssignmentHistory()
                            {
                                OBAssignmentID = assignmnt.oc.oc.OBAssignmentID,
                                MstOccurrenceBook = assignmnt.oc.oc.MstOccurrenceBook,
                                OBID = assignmnt.oc.oc.OBID,
                                MstEmployee = assignmnt.oc.oc.MstEmployee,
                                AssignedTO = assignmnt.oc.oc.AssignedTO,

                                CreatedOn = assignmnt.oc.oc.CreatedOn,
                                CreatedBy = assignmnt.oc.oc.CreatedBy,
                                UpdatedOn = assignmnt.oc.oc.UpdatedOn,
                                UpdatedBy = assignmnt.oc.oc.UpdatedBy,
                                IsDelete = assignmnt.oc.oc.IsDelete,
                                CreatedByName = (string.IsNullOrEmpty(assignmnt.oc.uc.FirstName) ? "" : (assignmnt.oc.uc.FirstName + " ")) + assignmnt.oc.uc.LastName ?? assignmnt.oc.uc.LastName,
                                UpdatedByName = (string.IsNullOrEmpty(assignmnt.um.FirstName) ? "" : (assignmnt.um.FirstName + " ")) + assignmnt.um.LastName,
                            })
                            .ApplySort(occurrenceBookAssignedHistory.OrderBy,
                            _propertyMappingService.GetPropertyMapping<OccurrenceBookForAssignmentDto, OccurrenceAssignmentHistory>());



            // var collectionBeforePaging =
            //     _context.OccurrenceAssignmentHistory
            //     .ApplySort(occurrenceBookAssignedHistory.OrderBy,
            //     _propertyMappingService.GetPropertyMapping<OccurrenceBookForAssignmentDto, OccurrenceAssignmentHistory>());

            if (!string.IsNullOrEmpty(occurrenceBookAssignedHistory.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookAssignedHistory.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                        //a.ReveiwComments.ToLowerInvariant().Contains(searchQueryForWhereClause)
                        a.CreatedByName.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                        a.UpdatedByName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<OccurrenceAssignmentHistory>.Create(collectionBeforePaging,
                occurrenceBookAssignedHistory.PageNumber,
                occurrenceBookAssignedHistory.PageSize);
        }
        #endregion OccurrenceAssignmentHistory

        #region OccurrenceReviewHistory
        public PagedList<OccurrenceReviewHistory> GetOccurrenceReviewHistories(Guid obid, OccurrenceBookReviewResourceParameters occurrenceBookReviewResourceParameters)
        {


            var collectionBeforePaging = _context.OccurrenceReviewHistory.Where(o => o.OBID == obid)
                    .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
                    .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
                        .Select(reviews => new OccurrenceReviewHistory()
                        {
                            OBReviewHistoryID = reviews.oc.oc.OBReviewHistoryID,
                            MstOccurrenceBook = reviews.oc.oc.MstOccurrenceBook,
                            OBID = reviews.oc.oc.OBID,
                            ReveiwComments = reviews.oc.oc.ReveiwComments,
                            CreatedOn = reviews.oc.oc.CreatedOn,
                            CreatedBy = reviews.oc.oc.CreatedBy,
                            UpdatedOn = reviews.oc.oc.UpdatedOn,
                            UpdatedBy = reviews.oc.oc.UpdatedBy,
                            IsDelete = reviews.oc.oc.IsDelete,
                            CreatedByName = (string.IsNullOrEmpty(reviews.oc.uc.FirstName) ? "" : (reviews.oc.uc.FirstName + " ")) + reviews.oc.uc.LastName ?? reviews.oc.uc.LastName,
                            UpdatedByName = (string.IsNullOrEmpty(reviews.um.FirstName) ? "" : (reviews.um.FirstName + " ")) + reviews.um.LastName,
                        })
                        .ApplySort(occurrenceBookReviewResourceParameters.OrderBy,
                        _propertyMappingService.GetPropertyMapping<OccurrenceBookReviewDto, OccurrenceReviewHistory>());

            if (!string.IsNullOrEmpty(occurrenceBookReviewResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookReviewResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                        a.ReveiwComments.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<OccurrenceReviewHistory>.Create(collectionBeforePaging,
                occurrenceBookReviewResourceParameters.PageNumber,
                occurrenceBookReviewResourceParameters.PageSize);
        }

        public OccurrenceReviewHistory GetReviewById(Guid occurrenceBookId, Guid reviewId)
        {
            var review = _context.OccurrenceReviewHistory.Where(a => a.OBReviewHistoryID == reviewId && a.OBID == occurrenceBookId)
                    .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
                    .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
                        .Select(reviews => new OccurrenceReviewHistory()
                        {
                            OBReviewHistoryID = reviews.oc.oc.OBReviewHistoryID,
                            MstOccurrenceBook = reviews.oc.oc.MstOccurrenceBook,
                            OBID = reviews.oc.oc.OBID,
                            ReveiwComments = reviews.oc.oc.ReveiwComments,
                            CreatedOn = reviews.oc.oc.CreatedOn,
                            CreatedBy = reviews.oc.oc.CreatedBy,
                            UpdatedOn = reviews.oc.oc.UpdatedOn,
                            UpdatedBy = reviews.oc.oc.UpdatedBy,
                            IsDelete = reviews.oc.oc.IsDelete,
                            CreatedByName = (string.IsNullOrEmpty(reviews.oc.uc.FirstName) ? "" : (reviews.oc.uc.FirstName + " ")) + reviews.oc.uc.LastName ?? reviews.oc.uc.LastName,
                            UpdatedByName = (string.IsNullOrEmpty(reviews.um.FirstName) ? "" : (reviews.um.FirstName + " ")) + reviews.um.LastName,
                        });
            return review.FirstOrDefault();
        }

        public void AddOccurrenceReviewHistories(OccurrenceReviewHistory occurrenceReviewHistory)
        {
            occurrenceReviewHistory.OBReviewHistoryID = Guid.NewGuid();
            _context.OccurrenceReviewHistory.Add(occurrenceReviewHistory);
        }
        #endregion OccurrenceReviewHistory

        #region Status
        public PagedList<OccurrenceStatusHistory> GetStatusHistory(Guid id, OccurrenceBookStatusResourceParameters occurrenceBookStatusHistoryParams)
        {
            // var collectionBeforePaging =
            //     _context.OccurrenceStatusHistory
            //     .ApplySort(occurrenceBookStatusHistoryParams.OrderBy,
            //     _propertyMappingService.GetPropertyMapping<OccurrenceBookStatusHistoryDto, OccurrenceStatusHistory>());
            var collectionBeforePaging = _context.OccurrenceStatusHistory.Where(s => s.OBID == id)
               .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
               .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
               .Select(status => new OccurrenceStatusHistory()
               {
                   OccurrenceStatusHistoryID = status.oc.oc.OccurrenceStatusHistoryID,
                   MstOccurrenceBook = status.oc.oc.MstOccurrenceBook,
                   OBID = status.oc.oc.OBID,
                   MstStatus = status.oc.oc.MstStatus,
                   StatusID = status.oc.oc.StatusID,
                   Comments = status.oc.oc.Comments,

                   CreatedOn = status.oc.oc.CreatedOn,
                   CreatedBy = status.oc.oc.CreatedBy,
                   UpdatedOn = status.oc.oc.UpdatedOn,
                   UpdatedBy = status.oc.oc.UpdatedBy,
                   IsDelete = status.oc.oc.IsDelete,
                   CreatedByName = (string.IsNullOrEmpty(status.oc.uc.FirstName) ? "" : (status.oc.uc.FirstName + " ")) + status.oc.uc.LastName ?? status.oc.uc.LastName,
                   UpdatedByName = (string.IsNullOrEmpty(status.um.FirstName) ? "" : (status.um.FirstName + " ")) + status.um.LastName,
               })
            .ApplySort(occurrenceBookStatusHistoryParams.OrderBy,
            _propertyMappingService.GetPropertyMapping<OccurrenceBookStatusHistoryDto, OccurrenceStatusHistory>());

            // var collectionBeforePaging =
            //      _context.OccurrenceStatusHistory
            //      .ApplySort(occurrenceBookStatusHistoryParams.OrderBy,
            //      _propertyMappingService.GetPropertyMapping<OccurrenceBookStatusHistoryDto, OccurrenceStatusHistory>());


            if (!string.IsNullOrEmpty(occurrenceBookStatusHistoryParams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = occurrenceBookStatusHistoryParams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a =>
                        a.Comments.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<OccurrenceStatusHistory>.Create(collectionBeforePaging,
                occurrenceBookStatusHistoryParams.PageNumber,
                occurrenceBookStatusHistoryParams.PageSize);
        }

        public void AddOccurrenceStatusHistory(OccurrenceStatusHistory occurrenceBookStatusHistory)
        {
            occurrenceBookStatusHistory.OccurrenceStatusHistoryID = Guid.NewGuid();
            _context.OccurrenceStatusHistory.Add(occurrenceBookStatusHistory);
        }
        #endregion status

        #region Employee History
        public void AddEmployeeAreaHistory(CfgEmployeeArea employeeAreaHistory)
        {
            employeeAreaHistory.EmployeeAreaID = Guid.NewGuid();
            _context.CfgEmployeeArea.Add(employeeAreaHistory);
        }
        public void AddEmployeeDepartmentHistory(CfgEmployeeDepartment employeeDepartmentHistory)
        {
            employeeDepartmentHistory.EmployeeDepartmentID = new Guid();
            _context.CfgEmployeeDepartment.Add(employeeDepartmentHistory);
        }
        public void AddEmployeeDesignationHistory(CfgEmployeeDesignation employeeDesignationHistory)
        {
            employeeDesignationHistory.EmployeeDesignationID = new Guid();
            _context.CfgEmployeeDesignation.Add(employeeDesignationHistory);
        }
        public void AddEmployeeShiftHistory(CfgEmployeeShift employeeShiftHistory)
        {
            employeeShiftHistory.EmployeeShiftID = new Guid();
            _context.CfgEmployeeShift.Add(employeeShiftHistory);
        }

        public PagedList<CfgEmployeeShift> GetEmployeeShiftHistory(Guid id, EmployeeShiftHistoryResourceParameters employeeStatusHistoryParams)
        {
            var collectionBeforePaging =
                _context.CfgEmployeeShift.Where(a => a.IsDelete == false && a.EmployeeID == id)
                .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
               .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
               .Select(status => new CfgEmployeeShift()
               {
                   EmployeeShiftID = status.oc.oc.EmployeeShiftID,
                   MstEmployee = status.oc.oc.MstEmployee,
                   EmployeeID = status.oc.oc.EmployeeID,
                   MstShift = status.oc.oc.MstShift,
                   ShiftID = status.oc.oc.ShiftID,

                   CreatedOn = status.oc.oc.CreatedOn,
                   CreatedBy = status.oc.oc.CreatedBy,
                   UpdatedOn = status.oc.oc.UpdatedOn,
                   UpdatedBy = status.oc.oc.UpdatedBy,
                   IsDelete = status.oc.oc.IsDelete,
                   CreatedByName = (string.IsNullOrEmpty(status.oc.uc.FirstName) ? "" : (status.oc.uc.FirstName + " ")) + status.oc.uc.LastName ?? status.oc.uc.LastName,
                   UpdatedByName = (string.IsNullOrEmpty(status.um.FirstName) ? "" : (status.um.FirstName + " ")) + status.um.LastName,
               })
                .ApplySort(employeeStatusHistoryParams.OrderBy,
                _propertyMappingService.GetPropertyMapping<EmployeeShiftHistoryDto, CfgEmployeeShift>());

            if (!string.IsNullOrEmpty(employeeStatusHistoryParams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = employeeStatusHistoryParams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MstShift.ShiftName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstEmployee.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstEmployee.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.CreatedByName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<CfgEmployeeShift>.Create(collectionBeforePaging,
                employeeStatusHistoryParams.PageNumber,
                employeeStatusHistoryParams.PageSize);
        }

        public PagedList<CfgEmployeeDepartment> GetEmployeeDepartmentHistory(Guid id, EmployeeDepartmentHistoryResourceParameters employeeDepartmentHistoryParams)
        {
            var collectionBeforePaging =
                        _context.CfgEmployeeDepartment.Where(a => a.IsDelete == false && a.EmployeeID == id)
                        .GroupJoin(_context.MstEmployee, oc => oc.CreatedBy.Value, uc => uc.EmployeeID, (oc, uc) => new { oc = oc, uc = uc.FirstOrDefault() })
                       .GroupJoin(_context.MstEmployee, oc => oc.oc.UpdatedBy, um => um.EmployeeID, (oc, um) => new { oc = oc, um = um.FirstOrDefault() })
                       .Select(status => new CfgEmployeeDepartment()
                       {
                           EmployeeDepartmentID = status.oc.oc.EmployeeDepartmentID,
                           MstEmployee = status.oc.oc.MstEmployee,
                           EmployeeID = status.oc.oc.EmployeeID,
                           MstDepartment = status.oc.oc.MstDepartment,
                           DepartmentID = status.oc.oc.DepartmentID,

                           CreatedOn = status.oc.oc.CreatedOn,
                           CreatedBy = status.oc.oc.CreatedBy,
                           UpdatedOn = status.oc.oc.UpdatedOn,
                           UpdatedBy = status.oc.oc.UpdatedBy,
                           IsDelete = status.oc.oc.IsDelete,
                           CreatedByName = (string.IsNullOrEmpty(status.oc.uc.FirstName) ? "" : (status.oc.uc.FirstName + " ")) + status.oc.uc.LastName ?? status.oc.uc.LastName,
                           UpdatedByName = (string.IsNullOrEmpty(status.um.FirstName) ? "" : (status.um.FirstName + " ")) + status.um.LastName,
                       })
                        .ApplySort(employeeDepartmentHistoryParams.OrderBy,
                        _propertyMappingService.GetPropertyMapping<EmployeeDepartmentHistoryDto, CfgEmployeeDepartment>());

            if (!string.IsNullOrEmpty(employeeDepartmentHistoryParams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = employeeDepartmentHistoryParams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.MstDepartment.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstEmployee.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.MstEmployee.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.CreatedByName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    );
            }

            return PagedList<CfgEmployeeDepartment>.Create(collectionBeforePaging,
                employeeDepartmentHistoryParams.PageNumber,
                employeeDepartmentHistoryParams.PageSize);
        }

        #endregion Employee History
    }
}
