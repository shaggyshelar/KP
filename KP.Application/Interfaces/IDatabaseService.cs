using KP.Domain.Department;
using Microsoft.EntityFrameworkCore;

namespace KP.Application.Interfaces
{
    public interface IDatabaseService
    {
        DbSet<Department> Departments { get; set; }

        void Save();
    }
}