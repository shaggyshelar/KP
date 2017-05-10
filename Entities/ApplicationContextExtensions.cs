using System;
using System.Collections.Generic;
using ESPL.KP.Entities.Core;
using ESPL.KP.Enums;
namespace ESPL.KP.Entities
{
    public static class ApplicationContextExtensions
    {
        public static void EnsureSeedDataForContext(this ApplicationContext context)
        {
            context.OccurrenceAssignmentHistory.RemoveRange(context.OccurrenceAssignmentHistory);
            context.OccurrenceReviewHistory.RemoveRange(context.OccurrenceReviewHistory);
            context.OccurrenceStatusHistory.RemoveRange(context.OccurrenceStatusHistory);
            context.MstOccurrenceBook.RemoveRange(context.MstOccurrenceBook);
            context.MstEmployee.RemoveRange(context.MstEmployee);
            context.MstArea.RemoveRange(context.MstArea);
            context.MstDepartment.RemoveRange(context.MstDepartment);
            context.MstDesignation.RemoveRange(context.MstDesignation);
            context.MstOccurrenceType.RemoveRange(context.MstOccurrenceType);
            context.MstStatus.RemoveRange(context.MstStatus);
            context.MstShift.RemoveRange(context.MstShift);
            context.AppModules.RemoveRange(context.AppModules);
            context.CfgEmployeeArea.RemoveRange(context.CfgEmployeeArea);
            context.CfgEmployeeDepartment.RemoveRange(context.CfgEmployeeDepartment);
            context.CfgEmployeeDesignation.RemoveRange(context.CfgEmployeeDesignation);
            context.CfgEmployeeShift.RemoveRange(context.CfgEmployeeShift);
            context.SaveChanges();

            UpdateDepartments(context);
            UpdateArea(context);
            UpdateDesignation(context);
            UpdateOccurrenceType(context);
            UpdateShifts(context);
            UpdateStatus(context);
            UpdateEmployee(context);
            UpdateOccurrenceBooks(context);
            UpdateOccurrenceBookAssignedToHistory(context);
            UpdateOccurrenceBookReviewHistory(context);
            UpdateOuccurrenceStatusHistory(context);
            UpdateAppModules(context);

        }

        private static void UpdateOuccurrenceStatusHistory(ApplicationContext context)
        {

            var occurrenceStatusHistory = new List<OccurrenceStatusHistory>(){
               new OccurrenceStatusHistory(){
                   OccurrenceStatusHistoryID=new Guid("2b72f829-5195-46c3-a6a4-06f817f12345"),
                   OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                   StatusID=new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    CreatedOn=DateTime.Now.AddHours(-6)
               },
               new OccurrenceStatusHistory(){
                   OccurrenceStatusHistoryID=new Guid("2b72f829-5195-46c3-a6a4-06f817f23456"),
                   OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                   StatusID=new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1111"),
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

               },
                new OccurrenceStatusHistory(){
                   OccurrenceStatusHistoryID=new Guid("2b72f829-5195-46c3-a6a4-06f817f34567"),
                   OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                   StatusID=new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1221"),
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

               },
                new OccurrenceStatusHistory(){
                   OccurrenceStatusHistoryID=new Guid("2b72f829-5195-46c3-a6a4-06f817f45678"),
                   OBID=new Guid("411bfab2-0d44-4fb9-8835-184db90f5678"),
                   StatusID=new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

               }
            };
            context.OccurrenceStatusHistory.AddRange(occurrenceStatusHistory);
            context.SaveChanges();
        }

        private static void UpdateDesignation(ApplicationContext context)
        {

            var designations = new List<MstDesignation>() {
                new MstDesignation() {
                    DesignationID = new Guid("2b72f829-5195-46c3-a6a4-06f817f11093"),
                    DesignationName = "Inspector-General",
                    DesignationCode = "IG",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("f6b0d655-5afd-44e1-a1d4-5d6bec3a7c81"),
                    DesignationName = "Deputy Inspector-General",
                    DesignationCode = "DIG",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("aff1592e-ba8e-4791-831c-5df49da69054"),
                    DesignationName = "Senior Assistant Inspector-General",
                    DesignationCode = "SAIG",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("15251460-e145-4aef-a3da-6846e881ad11"),
                    DesignationName = "Assistant Inspector-General",
                    DesignationCode = "AIG",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("6aac273a-ab24-4959-8c93-6f52cfee56ff"),
                    DesignationName = "Senior Superintendent",
                    DesignationCode = "SSP",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("1d45922a-a4ea-4d81-ad46-7227891199b1"),
                    DesignationName = "Superintendent",
                    DesignationCode = "SP",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("836bf2d2-7eb2-454a-a298-72a9d6aea480"),
                    DesignationName = "Assistant Superintendent",
                    DesignationCode = "ASP",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("7e08300b-0888-4789-964c-a70686c63b1d"),
                    DesignationName = "Chief Inspector",
                    DesignationCode = "CI",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("b6873249-6ee2-4506-97a6-cb0d9ce14aa9"),
                    DesignationName = "Inspector",
                    DesignationCode = "PI",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("ae573249-6ee2-4506-97a6-cb0d9ce14ca8"),
                    DesignationName = "Senior Sergeant",
                    DesignationCode = "SSGT",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("1f573249-6ee2-4506-97a6-cb0d9ce14ab9"),
                    DesignationName = "Sergeant",
                    DesignationCode = "SGT",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14896"),
                    DesignationName = "Constable",
                    DesignationCode = "CSTBL",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14897"),
                    DesignationName = "Super Admin",
                    DesignationCode = "SAdmin",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDesignation() {
                    DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14898"),
                    DesignationName = "Admin",
                    DesignationCode = "Admin",
                   CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                }
            };

            context.MstDesignation.AddRange(designations);
            context.SaveChanges();

        }

        private static void UpdateArea(ApplicationContext context)
        {
            #region Area

            var areas = new List<MstArea>() {
                new MstArea() {
                    AreaID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9579"),
                    AreaName = "Ladhri Awasi",
                    AreaCode = "LASI",
                    PinCode = "4 0122",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    AreaName = "Laikipia Campus",
                    AreaCode = "LKPC",
                    PinCode = "2 0330",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),
                    AreaName = "Laisamis",
                    AreaCode = "LSMS",
                    PinCode = "6 0502",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

                },
                new MstArea() {
                    AreaID = new Guid("901d24f6-f13e-4788-82f8-2416b11fe3f3"),
                    AreaName = "Lamu",
                    AreaCode = "LAMU",
                    PinCode = "8 0500",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

                },
                new MstArea() {
                    AreaID = new Guid("1d97702d-6d22-4256-a5ab-2844c2900fea"),
                    AreaName = "Lanet",
                    AreaCode = "LNET",
                    PinCode = "2 0112",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

                },
                new MstArea() {
                    AreaID = new Guid("717fb309-5cc9-422e-9b6e-28942ea181fa"),
                    AreaName = "Langas",
                    AreaCode = "LNGS",
                    PinCode = "3 0112",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("85d79042-0dc0-48f1-9e7a-2f39a5650290"),
                    AreaName = "Langata",
                    AreaCode = "LNGT",
                    PinCode = "0 0509",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("da00ca84-aff0-4b07-abd2-5777dd27be3d"),
                    AreaName = "Lavington",
                    AreaCode = "LVTN",
                    PinCode = "0 0603",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("49ce5ab0-2025-4c50-aaf8-587a44d1941e"),
                    AreaName = "Leshau",
                    AreaCode = "LSHU",
                    PinCode = "2 0310",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("08372fff-ad0b-40c8-8eed-595eab744ee8"),
                    AreaName = "Lessos",
                    AreaCode = "LSOS",
                    PinCode = "3 0302",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("2b401a2c-26c2-489e-835d-7473bb734783"),
                    AreaName = "Likoni",
                    AreaCode = "LKNI",
                    PinCode = "8 0110",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("46529153-730c-4971-bb28-76c5d2698bd8"),
                    AreaName = "Limuru",
                    AreaCode = "LMRU",
                    PinCode = "0 0217",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },

                new MstArea() {
                    AreaID = new Guid("8d4f017f-a130-4970-9928-7b8c10e029a0"),
                    AreaName = "Lita",
                    AreaCode = "LITA",
                    PinCode = "9 0109",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstArea() {
                    AreaID = new Guid("57c7f325-707b-4670-9afc-8a7707e47729"),
                    AreaName = "Litein",
                    AreaCode = "LTEN",
                    PinCode = "2 0210",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
            };

            context.MstArea.AddRange(areas);
            context.SaveChanges();
            #endregion
        }

        public static void UpdateDepartments(this ApplicationContext context)
        {


            var departments = new List<MstDepartment>() {
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471111"),
                    DepartmentName = "General Service Unit (GSU)",
                    DepartmentDespcription = "A paramilitary wing to deal with situations affecting internal security and to be a reserve force to deal with special operations and civil disorders.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709472222"),
                    DepartmentName = "Anti Stock Theft Unit",
                    DepartmentDespcription = "Anti Stock Theft Unit for stock ",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),
                    DepartmentName = "Criminal Investigation Department",
                    DepartmentDespcription = "Responsible for investigating complex cases.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709474444"),
                    DepartmentName = "Traffic Police Department",
                    DepartmentDespcription = "Force to enforce traffic laws in the republic.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709475555"),
                    DepartmentName = "Kenya Police College",
                    DepartmentDespcription = "Training College for Police cadets.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709476666"),
                    DepartmentName = "Kenya Police Air Wing",
                    DepartmentDespcription = "Provides air support and surveillance to troops on ground.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709477777"),
                    DepartmentName = "Kenya Railways Police",
                    DepartmentDespcription = "Maintaining law and order in trains and on train stations.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709478888"),
                    DepartmentName = "Kenya Police Dog Unit",
                    DepartmentDespcription = "Sniffer dogs to detect explosives and drugs.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },

                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709479999"),
                    DepartmentName = "Tourism Police Unit",
                    DepartmentDespcription = "A department to tackle crimes related to tourists.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471010"),
                    DepartmentName = "Kenya Airports Police Unit",
                    DepartmentDespcription = "A department tasked with protecting airports in the republic.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471011"),
                    DepartmentName = "Maritime Police Unit",
                    DepartmentDespcription = "A marine police unit to secure the coastline and internal rivers.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstDepartment() {
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471012"),
                    DepartmentName = "Diplomatic Police Unit",
                    DepartmentDespcription = "A department tasked with protecting the diplomats in the republic.",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
            };

            context.MstDepartment.AddRange(departments);
            context.SaveChanges();
        }

        private static void UpdateOccurrenceType(ApplicationContext context)
        {

            var occurrenceTypes = new List<MstOccurrenceType>() {
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf111"),
                    OBTypeName = "Carjacking",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf222"),
                    OBTypeName = "Theft and banditry",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf333"),
                    OBTypeName = "Ethnic violence",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf444"),
                    OBTypeName = "Corruption",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf555"),
                    OBTypeName = "Terrorism",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceType() {
                    OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf666"),
                    OBTypeName = "Drug abuse",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                }
            };
            context.MstOccurrenceType.AddRange(occurrenceTypes);
            context.SaveChanges();

        }

        #region Shifts

        public static void UpdateShifts(this ApplicationContext context)
        {

            var shifts = new List<MstShift>() {
                new MstShift() {
                    ShiftID = new Guid("318DC4DF-684A-444F-9E5A-18BB5EED1123"),
                    ShiftName = "Regular Morning Shift",
                    StartTime = TimeSpan.FromHours(4),
                    EndTime = TimeSpan.FromHours(12),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56285"),
                    ShiftName = "Officers Morning Shift",
                    StartTime = TimeSpan.FromHours(4),
                    EndTime = TimeSpan.FromHours(12),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56111"),
                    ShiftName = "Regular Mid-Day Shift",
                    StartTime = TimeSpan.FromHours(12),
                    EndTime = TimeSpan.FromHours(20),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90222"),
                    ShiftName = "Officers Mid-Day Shift",
                    StartTime = TimeSpan.FromHours(12),
                    EndTime = TimeSpan.FromHours(20),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90333"),
                    ShiftName = "Regular Night Shift",
                    StartTime = TimeSpan.FromHours(20),
                    EndTime = TimeSpan.FromHours(4),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90444"),
                    ShiftName = "Officers Night Shift",
                    StartTime = TimeSpan.FromHours(20),
                    EndTime = TimeSpan.FromHours(4),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstShift() {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90555"),
                    ShiftName = "Officers General Shift",
                    StartTime = TimeSpan.FromHours(8),
                    EndTime = TimeSpan.FromHours(4),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                }
            };


            context.MstShift.AddRange(shifts);
            context.SaveChanges();
        }
        #endregion


        #region Status

        public static void UpdateStatus(this ApplicationContext context)
        {
            var status = new List<MstStatus>() {
                new MstStatus() {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    StatusName = "Open",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                },
                new MstStatus() {
                    StatusID = new Guid("EBEED096-EA34-43E2-948E-32BB98F31401"),
                    StatusName = "Reviewed",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                },
                new MstStatus() {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1111"),
                    StatusName = "Assigned",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                },
                new MstStatus() {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1222"),
                    StatusName = "In Progress",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                },
                new MstStatus() {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1221"),
                    StatusName = "In Court",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                },
                new MstStatus() {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1223"),
                    StatusName = "Completed",
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn = DateTime.Now.AddHours(-6)
                }
            };


            context.MstStatus.AddRange(status);
            context.SaveChanges();
        }
        #endregion

        private static void UpdateOccurrenceBooks(ApplicationContext context)
        {

            var occurrenceBooks = new List<MstOccurrenceBook>() {
                new MstOccurrenceBook() {
                    OBID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		OBTypeID =  new Guid("758b1995-7f92-4d87-9588-b90800abf111"),	//carjacking
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		StatusID = new Guid("EBEED096-EA34-43E2-948E-32BB98F31401"),
                    OBNumber = "123456",
                    OBTime = Convert.ToDateTime("2017-04-20T19:23:14.9100866"),
                    CaseFileNumber = "1",
                    NatureOfOccurrence = "Preplanned.",
                    Remark = "total number of suspects: 4",
                    MapZoomLevel = 11,
                    Lattitude = 18.548026,
                    Longitude = 73.811683,
                    Location = "Near CSIR-URDIP",
                    AssignedTO = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef4444"),	//SGT
            		AssignedComments = "Assigned to a SGT in CID",
                    AssignedTime = DateTime.Now.AddDays(4),
                    Priority = OccurrencePriorities.Major,
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstOccurrenceBook() {
                    OBID = new Guid("411bfab2-0d44-4fb9-8835-184db90f5678"),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf222"),    //theft
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    OBNumber = "456789",
                    OBTime = Convert.ToDateTime("2017-04-10T19:25:14.9100866"),
                    CaseFileNumber = "2",
                    NatureOfOccurrence = "Nature 2",
                    Remark = "Test Remark 2",
                    MapZoomLevel = 11,
                    Lattitude = 18.550335,
                    Longitude = 73.809956,
                    Location = "Near IARIRS Baner",
                    AssignedTO = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"), //SAIG
            		AssignedComments = "Assigned to SAIG in CID",
                    AssignedTime = DateTime.Now.AddHours(-5),
                    Priority = OccurrencePriorities.Minor,
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    CreatedOn=DateTime.Now.AddHours(-5)
                },
                new MstOccurrenceBook() {
                    OBID = new Guid("411bfab2-0d44-4fb9-8835-184db90f8878"),
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),	//LKPC
            		OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf222"),	//theft
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709472222"),	//ASTU
            		StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243F1111"),
                    OBNumber = "789123",
                    OBTime = Convert.ToDateTime("2017-04-11T19:25:14.9100866"),
                    CaseFileNumber = "3",
                    NatureOfOccurrence = "Nature 3",
                    Remark = "Test Remark 3",
                    MapZoomLevel = 11,
                    Lattitude = 18.547787,
                    Longitude = 73.817699,
                    Location = "Near baner road",
                    AssignedTO =new Guid("56c385ae-ce46-41d4-b7fe-08df9aef2222"),	//DIG
            		AssignedComments = "Assigned",
                    AssignedTime = DateTime.Now.AddDays(-5),
                    Priority = OccurrencePriorities.Minor,
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    CreatedOn=DateTime.Now.AddHours(-4)
                },
                new MstOccurrenceBook() {
                    OBID = new Guid("411bfab2-0d44-4fb9-8835-184db90f7878"),
                    AreaID =  new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf333"),
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    OBNumber = "911119",
                    OBTime = Convert.ToDateTime("2017-04-15T21:25:14.9100866"),
                    CaseFileNumber = "4",
                    NatureOfOccurrence = "Nature 4",
                    Remark = "Test Remark 4",
                    AssignedTO = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),	//Constable
            		AssignedComments = "Test Assign to constable",
                    MapZoomLevel = 11,
                    Lattitude = 18.549613,
                    Longitude = 73.812145,
                    Location = "Near HDFC bank",
                    AssignedTime = DateTime.Now,
                    Priority = OccurrencePriorities.Critical,
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    CreatedOn=DateTime.Now.AddHours(-5)
                },
                new MstOccurrenceBook() {
                    OBID = new Guid("411bfab2-0d44-4fb9-8835-184db90f5545"),
                    AreaID =  new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		OBTypeID = new Guid("758b1995-7f92-4d87-9588-b90800abf333"),
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    OBNumber = "112211",
                    OBTime = Convert.ToDateTime("2017-04-15T21:25:14.9100866"),
                    CaseFileNumber = "5",
                    NatureOfOccurrence = "Nature 4",
                    Remark = "Test Remark 4",
                    AssignedTO =new Guid("56c385ae-ce46-41d4-b7fe-08df9aef5555"),	//Constable
            		AssignedComments = "Test Assign to constable",
                    MapZoomLevel = 11,
                    Lattitude = 18.551200,
                    Longitude = 73.813422,
                    Location = "Near The oval",
                    AssignedTime = DateTime.Now,
                    Priority = OccurrencePriorities.Major,
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    UpdatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-2)
                }

            };

            context.MstOccurrenceBook.AddRange(occurrenceBooks);
            context.SaveChanges();

        }

        public static void UpdateAppModules(this ApplicationContext context)
        {


            var appModules = new List<AppModule>() {
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a20f-55c269c12345"),
                    Name = "Department",
                    MenuText = "Department",
                    ShortName = "DP",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)

                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a22f-55c269c23456"),
                    Name = "Designation",
                    MenuText = "Designation",
                    ShortName = "DS",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c34567"),
                    Name = "Area",
                    MenuText = "Area",
                    ShortName = "AR",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c45678"),
                    Name = "Occurrence Type",
                    MenuText = "Occurrence Type",
                    ShortName = "OT",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c67891"),
                    Name = "Status",
                    MenuText = "Status",
                    ShortName = "ST",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c56889"),
                    Name = "Shift",
                    MenuText = "Shift",
                    ShortName = "SF",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c65412"),
                    Name = "Employee",
                    MenuText = "Employee",
                    ShortName = "EP",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c78912"),
                    Name = "Occurrence Books",
                    MenuText = "Occurrence Books",
                    ShortName = "OB",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c89123"),
                    Name = "Reports",
                    MenuText = "Reports",
                    ShortName = "RP",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c92345"),
                    Name = "Dashboard",
                    MenuText = "Dashboard",
                    ShortName = "DB",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c94567"),
                    Name = "Occurrence Assignment History",
                    MenuText = "Occurrence Assignment History",
                    ShortName = "OA",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new AppModule() {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c95678"),
                    Name = "Occurrence Review History",
                    MenuText = "Occurrence Review History",
                    ShortName = "OR",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
            };
            context.AppModules.AddRange(appModules);
            context.SaveChanges();
        }
        private static void UpdateEmployee(ApplicationContext context)
        {


            var employee = new List<MstEmployee>() {
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    FirstName = "John",
                    LastName = "Doe",
                    EmployeeCode = "Emp001",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9876543210",
                    Email = "john.doe@kenyapolice.com",
                    ResidencePhone = "020-22665544",
                    Address1 = "Westlands Commercial Centre, Ring Road",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9579"),	//Lasi
            		DesignationID = new Guid("2b72f829-5195-46c3-a6a4-06f817f11093"),	//IG
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471111"),	//GSU
            		ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90555"),			//General Officers
                    UserID= "56c385ae-ce46-41d4-b7fe-08df9aef7203",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef2222"),
                    FirstName = "Jack",
                    LastName = "Sparrow",
                    EmployeeCode = "Emp002",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9823654170",
                    Email = "jack.sparrow@kenyapolice.com",
                    ResidencePhone = "020-22665544",
                    Address1 = "Ngong, Olkeri, FOrest Line Road",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),	//LKPC
            		DesignationID = new Guid("f6b0d655-5afd-44e1-a1d4-5d6bec3a7c81"),	//DIG
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709472222"),	//ASTU
            		ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90555"),			//General Officers
                    UserID= "56c385ae-ce46-41d4-b7fe-08df9aef7202",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"),
                    FirstName = "Angelina",
                    LastName = "Jolie",
                    EmployeeCode = "Emp003",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Female",
                    Mobile = "95135782460",
                    Email = "angelina.jolie@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = "Salama House, Wabera Street Nairobi.",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		DesignationID = new Guid("aff1592e-ba8e-4791-831c-5df49da69054"),	//SAIG
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56285"),			//officers morning
                    UserID= "56c385ae-ce46-41d4-b7fe-08df9aef7201",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef4444"),
                    FirstName = "Brad",
                    LastName = "Pitt",
                    EmployeeCode = "Emp004",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9654781230",
                    Email = "brad.pitt@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = "Nejo plaza, Kasarani",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		DesignationID = new Guid("1f573249-6ee2-4506-97a6-cb0d9ce14ab9"),	//SGT
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90222"),
                    UserID= "56c385ae-ce46-41d4-b7fe-08df9aef7303",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef5555"),
                    FirstName = "Steve",
                    LastName = "Rogers",
                    EmployeeCode = "Emp005",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "8796541230",
                    Email = "steve.rogers@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = "Kilimani Business Centre,Kirichwa Road",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14896"),	//Constable
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE90333"),			//reg night
                    UserID = "56c385ae-ce46-41d4-b7fe-08df9aef7302",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666"),
                    FirstName = "Tony",
                    LastName = "Stark",
                    EmployeeCode = "Emp006",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9632587410",
                    Email = "tony.stark@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = " Limuru Rd/1st Parklands Ave, Parklands, Nairobi",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),	//LSMS
            		DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14896"),	//Constable
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56111"),		//reg mid day
                    UserID = "56c385ae-ce46-41d4-b7fe-08df9aef7301",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef7777"),
                    FirstName = "Johny",
                    LastName = "Depp",
                    EmployeeCode = "Emp007",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9632587412",
                    Email = "johny.depp@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = " Limuru Rd/1st Sandlands Ave, Sandlands, Nairobi",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),	//LKPC
            		DesignationID = new Guid("836bf2d2-7eb2-454a-a298-72a9d6aea480"),	//ASP
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56111"),		//reg mid day
                    UserID = "56c385ae-ce46-41d4-b7fe-08df9aef7204",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef8888"),
                    FirstName = "Nick",
                    LastName = "jones",
                    EmployeeCode = "Emp008",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9632587412",
                    Email = "nick.jones@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = " Limuru Rd/1st Sandlands Ave, Sandlands, Nairobi",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),	//LKPC
            		DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14897"),	//Admin
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56111"),		//reg mid day
                    UserID="56c385ae-ce46-41d4-b7fe-08df9aef7101",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },
                 new MstEmployee{
                    EmployeeID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9999"),
                    FirstName = "Tom",
                    LastName = "Cruise",
                    EmployeeCode = "Emp009",
                    DateOfBirth = DateTime.Now.AddYears(-30),
                    Gender = "Male",
                    Mobile = "9632587412",
                    Email = "tom.cruise@kenyapolice.com",
                    ResidencePhone = "020-22565784",
                    Address1 = " Limuru Rd/1st Sandlands Ave, Sandlands, Nairobi",
                    OrganizationJoiningDate = DateTime.Now.AddYears(-5),
                    ServiceJoiningDate = DateTime.Now.AddYears(-5),
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),	//LKPC
            		DesignationID = new Guid("57bf3249-6ee2-4506-97a6-cb0d9ce14898"),	//Admin
            		DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),	//CID
            		ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56111"),		//reg mid day
                    UserID="56c385ae-ce46-41d4-b7fe-08df9aef7102",
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    CreatedOn=DateTime.Now.AddHours(-6)
                },

            };

            context.MstEmployee.AddRange(employee);
            context.SaveChanges();
        }

        private static void UpdateOccurrenceBookAssignedToHistory(ApplicationContext context)
        {

            var occAssignmentHistory = new List<OccurrenceAssignmentHistory>() {
                new OccurrenceAssignmentHistory() {
                    OBAssignmentID=new Guid("56c385ae-ce46-41d4-b7fe-08df9ae12345"),
                    OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef4444"),
                    CreatedOn=DateTime.Now.AddHours(-1),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                },
                new OccurrenceAssignmentHistory() {
                    OBAssignmentID=new Guid("56c385ae-ce46-41d4-b7fe-08df9ae23456"),
                    OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"),
                    CreatedOn=DateTime.Now.AddHours(-2),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                },
                new OccurrenceAssignmentHistory() {
                    OBAssignmentID=new Guid("56c385ae-ce46-41d4-b7fe-08df9ae34567"),
                    OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef4444"),
                    CreatedOn=DateTime.Now.AddHours(-3),
                    CreatedBy=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                }
            };

            context.OccurrenceAssignmentHistory.AddRange(occAssignmentHistory);
            context.SaveChanges();
        }

        private static void UpdateOccurrenceBookReviewHistory(ApplicationContext context)
        {

            var occAssignmentHistory = new List<OccurrenceReviewHistory>() {
                new OccurrenceReviewHistory() {
                    OBReviewHistoryID = new Guid("56c385ae-ce46-41d4-b7fe-08df9ae12345"),
                    OBID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    ReveiwComments = "test review comments 1",
                    CreatedOn = DateTime.Now.AddHours(-1),
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                },
                new OccurrenceReviewHistory() {
                    OBReviewHistoryID = new Guid("56c385ae-ce46-41d4-b7fe-08df9ae23456"),
                    OBID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    ReveiwComments = "test review comments 2",
                    CreatedOn = DateTime.Now.AddHours(-2),
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                },
                new OccurrenceReviewHistory() {
                    OBReviewHistoryID = new Guid("56c385ae-ce46-41d4-b7fe-08df9ae34567"),
                    OBID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    ReveiwComments = "test review comments 3",
                    CreatedOn = DateTime.Now.AddHours(-3),
                    CreatedBy = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef6666")
                }
            };

            context.OccurrenceReviewHistory.AddRange(occAssignmentHistory);
            context.SaveChanges();
        }
    }
}
