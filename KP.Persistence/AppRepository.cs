using System;
using System.Collections.Generic;
using System.Linq;
using KP.Application.Departments;
using KP.Application.Interfaces;
using KP.Common.Helpers;
using KP.Common.Services;
using KP.Domain.Department;

namespace KP.Persistence
{
    public class AppRepository : IAppRepository
    {
        private ApplicationContext _context;
        private IPropertyMappingService _propertyMappingService;

        public AppRepository(ApplicationContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public PagedList<Department> GetDepartments(DepartmentsResourceParameters departmentResourceParameters)
        {
            var collectionBeforePaging =
                _context.Departments.Where(a => a.IsDelete == false).ApplySort(departmentResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<DepartmentDto, Department>());

            if (!string.IsNullOrEmpty(departmentResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = departmentResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.DepartmentName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || (a.DepartmentDespcription != null
                        && a.DepartmentDespcription.ToLowerInvariant().Contains(searchQueryForWhereClause)));
            }

            return PagedList<Department>.Create(collectionBeforePaging,
                departmentResourceParameters.PageNumber,
                departmentResourceParameters.PageSize);
        }

        public IEnumerable<LookUpItem> GetDepartmentAsLookUp()
        {
            return (from a in _context.Departments
                    where a.IsDelete == false
                    select new LookUpItem
                    {
                        ID = a.DepartmentID,
                        Name = a.DepartmentName
                    }).ToList();

        }

        public Department GetDepartment(Guid departmentId)
        {
            return _context.Departments.FirstOrDefault(a => a.DepartmentID == departmentId && a.IsDelete == false);
        }

        public IEnumerable<Department> GetDepartments(IEnumerable<Guid> departmentIds)
        {
            return _context.Departments.Where(a => departmentIds.Contains(a.DepartmentID) && a.IsDelete == false)
                .OrderBy(a => a.DepartmentName)
                .ToList();
        }

        public void AddDepartment(Department department)
        {
            department.DepartmentID = Guid.NewGuid();
            _context.Departments.Add(department);
        }

        public void DeleteDepartment(Department department)
        {
            _context.Departments.Remove(department);
        }

        public void UpdateDepartment(Department department)
        {
            // no code in this implementation
        }

        public bool DepartmentExists(Guid departmentId)
        {
            return _context.Departments.Any(a => a.DepartmentID == departmentId && a.IsDelete == false);
        }

    }
}