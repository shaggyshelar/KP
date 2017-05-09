using System;
using ESPL.KP.Models.Core;

namespace ESPL.KP.Models
{
    public class DepartmentForUpdationDto : BaseDto
    {
        public DepartmentForUpdationDto()
        {
        }
        public string DepartmentName { get; set; }
        public string DepartmentDespcription { get; set; }
    }
}