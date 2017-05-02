using System;
using ESPL.KP.Enums;

namespace ESPL.KP.Helpers.Core
{
    public static class Permissions
    {

        public const string DepartmentRead = "DP" + "." + "R";
        public const string DepartmentCreate = "DP" + "." + "C";
        public const string DepartmentUpdate = "DP" + "." + "U";
        public const string DepartmentDelete = "DP" + "." + "D";

        public const string DesignationRead = "DS" + "." + "R";
        public const string DesignationCreate = "DS" + "." + "C";
        public const string DesignationUpdate = "DS" + "." + "U";
        public const string DesignationDelete = "DS" + "." + "D";

        public const string AreaRead = "AR" + "." + "R";
        public const string AreaCreate = "AR" + "." + "C";
        public const string AreaUpdate = "AR" + "." + "U";
        public const string AreaDelete = "AR" + "." + "D";

        public const string OccurrenceTypeRead = "OT" + "." + "R";
        public const string OccurrenceTypeCreate = "OT" + "." + "C";
        public const string OccurrenceTypeUpdate = "OT" + "." + "U";
        public const string OccurrenceTypeDelete = "OT" + "." + "D";

        public const string StatusRead = "ST" + "." + "R";
        public const string StatusCreate = "ST" + "." + "C";
        public const string StatusUpdate = "ST" + "." + "U";
        public const string StatusDelete = "ST" + "." + "D";

        public const string ShiftRead = "SF" + "." + "R";
        public const string ShiftCreate = "SF" + "." + "C";
        public const string ShiftUpdate = "SF" + "." + "U";
        public const string ShiftDelete = "SF" + "." + "D";

        public const string EmployeeRead = "EP" + "." + "R";
        public const string EmployeeCreate = "EP" + "." + "C";
        public const string EmployeeUpdate = "EP" + "." + "U";
        public const string EmployeeDelete = "EP" + "." + "D";

        public const string OccurrenceBookRead = "OB" + "." + "R";
        public const string OccurrenceBookCreate = "OB" + "." + "C";
        public const string OccurrenceBookUpdate = "OB" + "." + "U";
        public const string OccurrenceBookDelete = "OB" + "." + "D";

        public const string ReportsRead = "RP" + "." + "R";
        public const string ReportsCreate = "RP" + "." + "C";
        public const string ReportsUpdate = "RP" + "." + "U";
        public const string ReportsDelete = "RP" + "." + "D";

        public const string DashboardDelete = "DB" + "." + "D";
        public const string DashboardRead = "DB" + "." + "R";
        public const string DashboardCreate = "DB" + "." + "C";
        public const string DashboardUpdate = "DB" + "." + "U";



        // public static string DepartmentRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DepartmentShortName, PermissionType.R);
        //     }
        // }
        // public static string DepartmentCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DepartmentShortName, PermissionType.R);
        //     }
        // }
        // public static string DepartmentUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DepartmentShortName, PermissionType.U);
        //     }
        // }
        // public static string DepartmentDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DepartmentShortName, PermissionType.D);
        //     }
        // }

        // public static string DesignationRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DesignationShortName, PermissionType.R);
        //     }
        // }
        // public static string DesignationCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DesignationShortName, PermissionType.R);
        //     }
        // }
        // public static string DesignationUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DesignationShortName, PermissionType.U);
        //     }
        // }
        // public static string DesignationDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DesignationShortName, PermissionType.D);
        //     }
        // }

        // public static string AreaRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.AreaShortName, PermissionType.R);
        //     }
        // }
        // public static string AreaCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.AreaShortName, PermissionType.R);
        //     }
        // }
        // public static string AreaUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.AreaShortName, PermissionType.U);
        //     }
        // }
        // public static string AreaDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.AreaShortName, PermissionType.D);
        //     }
        // }

        // public static string OccurrenceTypeRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceTypeShortName, PermissionType.R);
        //     }
        // }
        // public static string OccurrenceTypeCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceTypeShortName, PermissionType.R);
        //     }
        // }
        // public static string OccurrenceTypeUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceTypeShortName, PermissionType.U);
        //     }
        // }
        // public static string OccurrenceTypeDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceTypeShortName, PermissionType.D);
        //     }
        // }

        // public static string StatusRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.StatusShortName, PermissionType.R);
        //     }
        // }
        // public static string StatusCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.StatusShortName, PermissionType.R);
        //     }
        // }
        // public static string StatusUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.StatusShortName, PermissionType.U);
        //     }
        // }
        // public static string StatusDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.StatusShortName, PermissionType.D);
        //     }
        // }
        // public static string ShiftRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ShiftShortName, PermissionType.R);
        //     }
        // }
        // public static string ShiftCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ShiftShortName, PermissionType.R);
        //     }
        // }
        // public static string ShiftUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ShiftShortName, PermissionType.U);
        //     }
        // }
        // public static string ShiftDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ShiftShortName, PermissionType.D);
        //     }
        // }

        // public static string EmployeeRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.EmployeeShortName, PermissionType.R);
        //     }
        // }
        // public static string EmployeeCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.EmployeeShortName, PermissionType.R);
        //     }
        // }
        // public static string EmployeeUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.EmployeeShortName, PermissionType.U);
        //     }
        // }
        // public static string EmployeeDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.EmployeeShortName, PermissionType.D);
        //     }
        // }

        // public static string OccurrenceBookRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceBookShortName, PermissionType.R);
        //     }
        // }
        // public static string OccurrenceBookCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceBookShortName, PermissionType.R);
        //     }
        // }
        // public static string OccurrenceBookUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceBookShortName, PermissionType.U);
        //     }
        // }
        // public static string OccurrenceBookDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.OccurrenceBookShortName, PermissionType.D);
        //     }
        // }
        // public static string ReportsRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ReportsShortName, PermissionType.R);
        //     }
        // }
        // public static string ReportsCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ReportsShortName, PermissionType.R);
        //     }
        // }
        // public static string ReportsUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ReportsShortName, PermissionType.U);
        //     }
        // }
        // public static string ReportsDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.ReportsShortName, PermissionType.D);
        //     }
        // }
        // public static string DashboardRead
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DashboardShortName, PermissionType.R);
        //     }
        // }
        // public static string DashboardCreate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DashboardShortName, PermissionType.R);
        //     }
        // }
        // public static string DashboardUpdate
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DashboardShortName, PermissionType.U);
        //     }
        // }
        // public static string DashboardDelete
        // {
        //     get
        //     {
        //         return string.Format("{0}" + "." + "{1}", Modules.DashboardShortName, PermissionType.D);
        //     }
        // }
    }
}
