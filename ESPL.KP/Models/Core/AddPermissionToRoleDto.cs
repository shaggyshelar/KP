using System;
using ESPL.KP.Enums;

namespace ESPL.KP.Models.Core
{
    public class AddPermissionToRoleDto
    {
        public string AppModuleName { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}