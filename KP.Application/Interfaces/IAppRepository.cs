using System;
using System.Collections.Generic;
using KP.Application.Departments;
using KP.Common.Helpers;
using KP.Domain.Department;

namespace KP.Application.Interfaces
{
    public interface IAppRepository
    {
        #region Department
        PagedList<Department> GetDepartments(DepartmentsResourceParameters departmentResourceParameters);
        Department GetDepartment(Guid departmentId);
        IEnumerable<Department> GetDepartments(IEnumerable<Guid> departmentIds);
        void AddDepartment(Department department);
        void DeleteDepartment(Department department);
        void UpdateDepartment(Department department);
        bool DepartmentExists(Guid departmentId);

        #endregion
    }
}