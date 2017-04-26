using System;
using System.Collections.Generic;
using ESPL.KP.Entities.Core;

namespace ESPL.KP.Entities
{
    public static class LibraryContextExtensions
    {
        public static void EnsureSeedDataForContext(this LibraryContext context)
        {
            // first, clear the database.  This ensures we can always start 
            // fresh with each demo.  Not advised for production environments, obviously :-)
            context.Authors.RemoveRange(context.Authors);
            context.SaveChanges();

            // init seed data
            var authors = new List<Author>()
            {
                new Author()
                {
                     Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                     FirstName = "Stephen",
                     LastName = "King",
                     Genre = "Horror",
                     DateOfBirth = new DateTimeOffset(new DateTime(1947, 9, 21)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("c7ba6add-09c4-45f8-8dd0-eaca221e5d93"),
                             Title = "The Shining",
                             Description = "The Shining is a horror novel by American author Stephen King. Published in 1977, it is King's third published novel and first hardback bestseller: the success of the book firmly established King as a preeminent author in the horror genre. "
                         },
                         new Book()
                         {
                             Id = new Guid("a3749477-f823-4124-aa4a-fc9ad5e79cd6"),
                             Title = "Misery",
                             Description = "Misery is a 1987 psychological horror novel by Stephen King. This novel was nominated for the World Fantasy Award for Best Novel in 1988, and was later made into a Hollywood film and an off-Broadway play of the same name."
                         },
                         new Book()
                         {
                             Id = new Guid("70a1f9b9-0a37-4c1a-99b1-c7709fc64167"),
                             Title = "It",
                             Description = "It is a 1986 horror novel by American author Stephen King. The story follows the exploits of seven children as they are terrorized by the eponymous being, which exploits the fears and phobias of its victims in order to disguise itself while hunting its prey. 'It' primarily appears in the form of a clown in order to attract its preferred prey of young children."
                         },
                         new Book()
                         {
                             Id = new Guid("60188a2b-2784-4fc4-8df8-8919ff838b0b"),
                             Title = "The Stand",
                             Description = "The Stand is a post-apocalyptic horror/fantasy novel by American author Stephen King. It expands upon the scenario of his earlier short story 'Night Surf' and outlines the total breakdown of society after the accidental release of a strain of influenza that had been modified for biological warfare causes an apocalyptic pandemic which kills off the majority of the world's human population."
                         }
                     }
                },
                new Author()
                {
                     Id = new Guid("76053df4-6687-4353-8937-b45556748abe"),
                     FirstName = "George",
                     LastName = "RR Martin",
                     Genre = "Fantasy",
                     DateOfBirth = new DateTimeOffset(new DateTime(1948, 9, 20)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("447eb762-95e9-4c31-95e1-b20053fbe215"),
                             Title = "A Game of Thrones",
                             Description = "A Game of Thrones is the first novel in A Song of Ice and Fire, a series of fantasy novels by American author George R. R. Martin. It was first published on August 1, 1996."
                         },
                         new Book()
                         {
                             Id = new Guid("bc4c35c3-3857-4250-9449-155fcf5109ec"),
                             Title = "The Winds of Winter",
                             Description = "Forthcoming 6th novel in A Song of Ice and Fire."
                         },
                         new Book()
                         {
                             Id = new Guid("09af5a52-9421-44e8-a2bb-a6b9ccbc8239"),
                             Title = "A Dance with Dragons",
                             Description = "A Dance with Dragons is the fifth of seven planned novels in the epic fantasy series A Song of Ice and Fire by American author George R. R. Martin."
                         }
                     }
                },
                new Author()
                {
                     Id = new Guid("412c3012-d891-4f5e-9613-ff7aa63e6bb3"),
                     FirstName = "Neil",
                     LastName = "Gaiman",
                     Genre = "Fantasy",
                     DateOfBirth = new DateTimeOffset(new DateTime(1960, 11, 10)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("9edf91ee-ab77-4521-a402-5f188bc0c577"),
                             Title = "American Gods",
                             Description = "American Gods is a Hugo and Nebula Award-winning novel by English author Neil Gaiman. The novel is a blend of Americana, fantasy, and various strands of ancient and modern mythology, all centering on the mysterious and taciturn Shadow."
                         }
                     }
                },
                new Author()
                {
                     Id = new Guid("578359b7-1967-41d6-8b87-64ab7605587e"),
                     FirstName = "Sagar",
                     LastName = "Shelar",
                     Genre = "Various",
                     DateOfBirth = new DateTimeOffset(new DateTime(1958, 8, 27)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("01457142-358f-495f-aafa-fb23de3d67e9"),
                             Title = "Speechless",
                             Description = "Good-natured and often humorous, Speechless is at times a 'song of curses', as Lanoye describes the conflicts with his beloved diva of a mother and her brave struggle with decline and death."
                         }
                     }
                },
                new Author()
                {
                     Id = new Guid("f74d6899-9ed2-4137-9876-66b070553f8f"),
                     FirstName = "Douglas",
                     LastName = "Adams",
                     Genre = "Science fiction",
                     DateOfBirth = new DateTimeOffset(new DateTime(1952, 3, 11)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("e57b605f-8b3c-4089-b672-6ce9e6d6c23f"),
                             Title = "The Hitchhiker's Guide to the Galaxy",
                             Description = "The Hitchhiker's Guide to the Galaxy is the first of five books in the Hitchhiker's Guide to the Galaxy comedy science fiction 'trilogy' by Douglas Adams."
                         }
                     }
                },
                new Author()
                {
                     Id = new Guid("a1da1d8e-1988-4634-b538-a01709477b77"),
                     FirstName = "Jens",
                     LastName = "Lapidus",
                     Genre = "Thriller",
                     DateOfBirth = new DateTimeOffset(new DateTime(1974, 5, 24)),
                     Books = new List<Book>()
                     {
                         new Book()
                         {
                             Id = new Guid("1325360c-8253-473a-a20f-55c269c20407"),
                             Title = "Easy Money",
                             Description = "Easy Money or Snabba cash is a novel from 2006 by Jens Lapidus. It has been a success in term of sales, and the paperback was the fourth best seller of Swedish novels in 2007."
                         }
                     }
                }
            };

            context.Authors.AddRange(authors);
            context.SaveChanges();




            #region Shift
            context.MstShift.RemoveRange(context.MstShift);
            context.SaveChanges();
            var shifts = new List<MstShift>()
            {
                new MstShift()
                {
                    ShiftID = new Guid("318DC4DF-684A-444F-9E5A-18BB5EED1123"),
                    ShiftName = "Shift1",
                    StartTime = TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                },
                new MstShift()
                {
                    ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56285"),
                    ShiftName = "Shift2",
                    StartTime = TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                },
                new MstShift()
                {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE904CA"),
                    ShiftName = "Shift3",
                    StartTime =TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                }
            };

            context.MstShift.AddRange(shifts);
            context.SaveChanges();
            #endregion

            UpdateDepartments(context);
            UpdateArea(context);
            UpdateDesignation(context);
            UpdateOccurrenceType(context);
            UpdateShifts(context);
            UpdateStatus(context);
            UpdateEmployee(context);
            UpdateOccurrenceBooks(context);
            UpdateAppModules(context);

        }



        private static void UpdateDesignation(LibraryContext context)
        {
            context.MstDesignation.RemoveRange(context.MstDesignation);
            context.SaveChanges();
            var designations = new List<MstDesignation>()
            {
                new MstDesignation()
                {
                    DesignationID = new Guid("2b72f829-5195-46c3-a6a4-06f817f11093"),
                    DesignationName = "Director general of police",
                    DesignationCode = "DGP"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("f6b0d655-5afd-44e1-a1d4-5d6bec3a7c81"),
                    DesignationName = "Inspector General of Police",
                    DesignationCode = "IGP"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("aff1592e-ba8e-4791-831c-5df49da69054"),
                    DesignationName = "Deputy Inspector General of Police",
                    DesignationCode = "DIG"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("15251460-e145-4aef-a3da-6846e881ad11"),
                    DesignationName = "Deputy Commissioner of Police",
                    DesignationCode = "DCP"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("6aac273a-ab24-4959-8c93-6f52cfee56ff"),
                    DesignationName = "Assistant Commissioner of Police",
                    DesignationCode = "DCP"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("1d45922a-a4ea-4d81-ad46-7227891199b1"),
                    DesignationName = "Superintendent of police",
                    DesignationCode = "SP"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("836bf2d2-7eb2-454a-a298-72a9d6aea480"),
                    DesignationName = "Police Inspector",
                    DesignationCode = "PI"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("7e08300b-0888-4789-964c-a70686c63b1d"),
                    DesignationName = "Police SubInspector",
                    DesignationCode = "PSI"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("bf573249-6ee2-4506-97a6-cb0d9ce14ca8"),
                    DesignationName = "Police Head Constable",
                    DesignationCode = "HPC"
                }
            };

            context.MstDesignation.AddRange(designations);
            context.SaveChanges();

        }

        private static void UpdateArea(LibraryContext context)
        {
            #region Area
            context.MstArea.RemoveRange(context.MstArea);
            context.SaveChanges();
            var areas = new List<MstArea>()
            {
                new MstArea()
                {
                    AreaID = new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9579"),
                    AreaName = "Area1",
                    AreaCode = "A1",
                    PinCode = "1000"
                },
                new MstArea()
                {
                    AreaID = new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    AreaName = "Area2",
                    AreaCode = "A2",
                    PinCode = "1001"
                },
                new MstArea()
                {
                    AreaID = new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),
                    AreaName = "Area3",
                    AreaCode = "A3",
                    PinCode = "1002"

                },
                new MstArea()
                {
                    AreaID = new Guid("901d24f6-f13e-4788-82f8-2416b11fe3f3"),
                    AreaName = "Area4",
                    AreaCode = "A4",
                    PinCode = "1003"

                },
                new MstArea()
                {
                   AreaID = new Guid("1d97702d-6d22-4256-a5ab-2844c2900fea"),
                   AreaName = "Area5",
                   AreaCode = "A5",
                   PinCode = "1004"

                },
                new MstArea()
                {
                  AreaID = new Guid("717fb309-5cc9-422e-9b6e-28942ea181fa"),
                  AreaName = "Area6",
                  AreaCode = "A6",
                  PinCode = "1005"
                },
                new MstArea()
                {
                   AreaID = new Guid("85d79042-0dc0-48f1-9e7a-2f39a5650290"),
                   AreaName = "Area7",
                   AreaCode = "A7",
                   PinCode = "1006"
                },
                new MstArea()
                {
                   AreaID = new Guid("da00ca84-aff0-4b07-abd2-5777dd27be3d"),
                   AreaName = "Area8",
                   AreaCode = "A8",
                   PinCode = "1007"
                },
                new MstArea()
                {
                   AreaID = new Guid("49ce5ab0-2025-4c50-aaf8-587a44d1941e"),
                   AreaName = "Area9",
                   AreaCode = "A9",
                   PinCode = "1008"
                },
                new MstArea()
                {
                    AreaID = new Guid("08372fff-ad0b-40c8-8eed-595eab744ee8"),
                    AreaName = "Area10",
                    AreaCode = "A10",
                    PinCode = "1009"
                },
                new MstArea()
                {
                  AreaID = new Guid("2b401a2c-26c2-489e-835d-7473bb734783"),
                  AreaName = "Area11",
                  AreaCode = "A11",
                  PinCode = "1010"
                },
                new MstArea()
                {
                   AreaID = new Guid("46529153-730c-4971-bb28-76c5d2698bd8"),
                   AreaName = "Area12",
                   AreaCode = "A12",
                   PinCode = "1011"
                },

                new MstArea()
                {
                   AreaID = new Guid("8d4f017f-a130-4970-9928-7b8c10e029a0"),
                   AreaName = "Area13",
                   AreaCode = "A13",
                   PinCode = "1012"
                },
                new MstArea()
                {
                   AreaID = new Guid("57c7f325-707b-4670-9afc-8a7707e47729"),
                   AreaName = "Area14",
                   AreaCode = "A14",
                    PinCode = "1013"
                },


            };

            context.MstArea.AddRange(areas);
            context.SaveChanges();
            #endregion
        }

        public static void UpdateDepartments(this LibraryContext context)
        {
            context.MstDepartment.RemoveRange(context.MstDepartment);
            context.SaveChanges();

            var departments = new List<MstDepartment>(){
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709471111"),
                    DepartmentName = "Department 1",
                    DepartmentDespcription = "Description for department 1",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709472222"),
                    DepartmentName = "Department 2",
                    DepartmentDespcription = "Description for department 2",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709473333"),
                    DepartmentName = "Department 3",
                    DepartmentDespcription = "Description for department 3",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709474444"),
                    DepartmentName = "Department 4",
                    DepartmentDespcription = "Description for department 4",
                },new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709475555"),
                    DepartmentName = "Department 5",
                    DepartmentDespcription = "Description for department 5",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709476666"),
                    DepartmentName = "Department 6",
                    DepartmentDespcription = "Description for department 6",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709477777"),
                    DepartmentName = "Department 7",
                    DepartmentDespcription = "Description for department 7",
                },
                new MstDepartment(){
                    DepartmentID = new Guid("a1da1d8e-1111-4634-b538-a01709478888"),
                    DepartmentName = "Department 8",
                    DepartmentDespcription = "Description for department 8",
                }
            };

            context.MstDepartment.AddRange(departments);
            context.SaveChanges();
        }

        private static void UpdateOccurrenceType(LibraryContext context)
        {
            context.MstOccurrenceType.RemoveRange(context.MstOccurrenceType);
            context.SaveChanges();
            var occurrenceTypes = new List<MstOccurrenceType>() {
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf111"),OBTypeName="Occurrence Type 1"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf222"),OBTypeName="Occurrence Type 2"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf333"),OBTypeName="Occurrence Type 3"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf444"),OBTypeName="Occurrence Type 4"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf555"),OBTypeName="Occurrence Type 5"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf666"),OBTypeName="Occurrence Type 6"},
                new MstOccurrenceType(){OBTypeID=new Guid("758b1995-7f92-4d87-9588-b90800abf777"),OBTypeName="Occurrence Type 7"}
            };
            context.MstOccurrenceType.AddRange(occurrenceTypes);
            context.SaveChanges();

        }

        #region Shifts

        public static void UpdateShifts(this LibraryContext context)
        {
            context.MstShift.RemoveRange(context.MstShift);
            context.SaveChanges();
            var shifts = new List<MstShift>()
            {
                new MstShift()
                {
                    ShiftID = new Guid("318DC4DF-684A-444F-9E5A-18BB5EED1123"),
                    ShiftName = "Shift1",
                    StartTime = TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                },
                new MstShift()
                {
                    ShiftID = new Guid("95998825-255A-401F-AAB1-5EF4C2A56285"),
                    ShiftName = "Shift2",
                    StartTime = TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                },
                new MstShift()
                {
                    ShiftID = new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE904CA"),
                    ShiftName = "Shift3",
                    StartTime =TimeSpan.FromMinutes(1),
                    EndTime = TimeSpan.FromMinutes(1)
                }
            };

            context.MstShift.AddRange(shifts);
            context.SaveChanges();
        }
        #endregion


        #region Status

        public static void UpdateStatus(this LibraryContext context)
        {
            context.MstStatus.RemoveRange(context.MstStatus);
            context.SaveChanges();
            var status = new List<MstStatus>()
            {
                new MstStatus()
                {
                    StatusID = new Guid("1DD5458B-E136-4D03-B309-0089D4A9BD9D"),
                    StatusName = "New"
                },
                new MstStatus()
                {
                    StatusID = new Guid("EBEED096-EA34-43E2-948E-32BB98F31401"),
                    StatusName = "Assigned"
                },
                new MstStatus()
                {
                    StatusID = new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    StatusName = "Reviewed"
                }
            };

            context.MstStatus.AddRange(status);
            context.SaveChanges();
        }
        #endregion

        private static void UpdateOccurrenceBooks(LibraryContext context)
        {
            context.MstOccurrenceBook.RemoveRange(context.MstOccurrenceBook);
            context.SaveChanges();
            var occurrenceBooks = new List<MstOccurrenceBook>() {
                new MstOccurrenceBook() {
                    OBID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1234"),
                    AreaID= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9579"),
                    OBTypeID= new Guid("758b1995-7f92-4d87-9588-b90800abf111"),
                    DepartmentID= new Guid("a1da1d8e-1111-4634-b538-a01709471111"),
                    StatusID= new Guid("1DD5458B-E136-4D03-B309-0089D4A9BD9D"),
                    OBNumber= "123",
                    OBTime= Convert.ToDateTime("2017-04-20T19:23:14.9100866"),
                    CaseFileNumber= "1",
                    NatureOfOccurrence= "Nature 1",
                    Remark= "Test Remark 1",
                    MapZoomLevel =11,
                    Lattitude=18.555905,
                    Longitude=73.805589,
                    Location="Near Agarrwal Packers and Movers",
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    AssignedComments= "Assigned",
                    AssignedTime= DateTime.Now.AddDays(4)
                  },
                new MstOccurrenceBook()  {
                    OBID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f5678"),
                    AreaID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    OBTypeID=new Guid( "758b1995-7f92-4d87-9588-b90800abf222"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709472222"),
                    StatusID=new Guid("EBEED096-EA34-43E2-948E-32BB98F31401"),
                    OBNumber= "456",
                    OBTime= Convert.ToDateTime("2017-04-10T19:25:14.9100866"),
                    CaseFileNumber= "2",
                    NatureOfOccurrence= "Nature 2",
                    Remark= "Test Remark 2",
                    MapZoomLevel =11,
                    Lattitude=18.555905,
                    Longitude=73.805589,
                    Location="Near Agarrwal Packers and Movers",
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef2222"),
                    AssignedComments= "Assigned",
                    AssignedTime=DateTime.Now.AddHours(-5)
                  },
                  new MstOccurrenceBook()  {
                    OBID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f8878"),
                    AreaID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    OBTypeID=new Guid( "758b1995-7f92-4d87-9588-b90800abf222"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709473333"),
                    StatusID=new Guid("EBEED096-EA34-43E2-948E-32BB98F31401"),
                    OBNumber= "888",
                    OBTime= Convert.ToDateTime("2017-04-11T19:25:14.9100866"),
                    CaseFileNumber= "3",
                    NatureOfOccurrence= "Nature 3",
                    Remark= "Test Remark 3",
                    MapZoomLevel =11,
                    Lattitude=18.555905,
                    Longitude=73.805589,
                    Location="Near Agarrwal Packers and Movers",
                    AssignedTO= new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"),
                    AssignedComments= "Assigned",
                    AssignedTime= DateTime.Now.AddDays(-5)
                  },
                  new MstOccurrenceBook()  {
                    OBID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f7878"),
                    AreaID=new Guid( "411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    OBTypeID=new Guid( "758b1995-7f92-4d87-9588-b90800abf222"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709472222"),
                    StatusID=new Guid("853BDECF-1ED1-46C4-B200-E8BE243FDDAD"),
                    OBNumber= "911",
                    OBTime= Convert.ToDateTime("2017-04-15T21:25:14.9100866"),
                    CaseFileNumber= "4",
                    NatureOfOccurrence= "Nature 4",
                    Remark= "Test Remark 4",
                    AssignedTO=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"),
                    AssignedComments="Test Assign to officer",
                    MapZoomLevel =11,
                    Lattitude=18.555905,
                    Longitude=73.805589,
                    Location="Near Agarrwal Packers and Movers",
                    AssignedTime=DateTime.Now
                  }

            };
            context.MstOccurrenceBook.AddRange(occurrenceBooks);
            context.SaveChanges();

        }

        public static void UpdateAppModules(this LibraryContext context)
        {
            context.AppModules.RemoveRange(context.AppModules);
            context.SaveChanges();

            var appModules = new List<AppModule>()
            {
                new AppModule()
                {
                    Id = new Guid("1325360c-8253-473a-a20f-55c269c20407"),
                    Name = "Department",
                    MenuText = "Department"
                },
                new AppModule()
                {
                    Id = new Guid("1325360c-8253-473a-a22f-55c269c20407"),
                    Name = "Designation",
                    MenuText = "Designation"
                },
                new AppModule()
                {
                    Id = new Guid("1325360c-8253-473a-a23f-55c269c20407"),
                    Name = "Area",
                    MenuText = "Area"
                }
            };

            context.AppModules.AddRange(appModules);
            context.SaveChanges();
        }
        private static void UpdateEmployee(LibraryContext context)
        {
            context.MstEmployee.RemoveRange(context.MstEmployee);
            context.SaveChanges();
            var employee = new List<MstEmployee>()
            {
                new MstEmployee
                {
                    EmployeeID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef1111"),
                    FirstName="First Name1",
                    LastName="Last Name1",
                    EmployeeCode="Emp001",
                    DateOfBirth=DateTime.Now.AddYears(-30),
                    Gender="Male",
                    Mobile="1234567890",
                    Email="Test1@test.com",
                    ResidencePhone="020-22665544",
                    Address1="Test Address 1",
                    OrganizationJoiningDate=DateTime.Now.AddYears(-5),
                    ServiceJoiningDate=DateTime.Now.AddYears(-5),
                    AreaID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef9579"),
                    DesignationID=new Guid("2b72f829-5195-46c3-a6a4-06f817f11093"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709471111"),
                    ShiftID= new Guid("318DC4DF-684A-444F-9E5A-18BB5EED1123")
                },
                 new MstEmployee
                {
                    EmployeeID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef2222"),
                    FirstName="First Name1",
                    LastName="Last Name1",
                    EmployeeCode="Emp001",
                    DateOfBirth=DateTime.Now.AddYears(-30),
                    Gender="Male",
                    Mobile="1234567890",
                    Email="Test1@test.com",
                    ResidencePhone="020-22665544",
                    Address1="Test Address 2",
                    OrganizationJoiningDate=DateTime.Now.AddYears(-5),
                    ServiceJoiningDate=DateTime.Now.AddYears(-5),
                    AreaID=new Guid("411bfab2-0d44-4fb9-8835-184db90f44fa"),
                    DesignationID=new Guid("f6b0d655-5afd-44e1-a1d4-5d6bec3a7c81"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709472222"),
                    ShiftID= new Guid("95998825-255A-401F-AAB1-5EF4C2A56285")
                },
                 new MstEmployee
                {
                    EmployeeID=new Guid("56c385ae-ce46-41d4-b7fe-08df9aef3333"),
                    FirstName="First Name3",
                    LastName="Last Name3",
                    EmployeeCode="Emp003",
                    DateOfBirth=DateTime.Now.AddYears(-30),
                    Gender="Female",
                    Mobile="12344564644",
                    Email="Test2@test.com",
                    ResidencePhone="020-22565784",
                    Address1="Test Address 3",
                    OrganizationJoiningDate=DateTime.Now.AddYears(-5),
                    ServiceJoiningDate=DateTime.Now.AddYears(-5),
                    AreaID=new Guid("89234f93-6a6a-4960-a7d3-20f98f2760a8"),
                    DesignationID=new Guid("aff1592e-ba8e-4791-831c-5df49da69054"),
                    DepartmentID=new Guid("a1da1d8e-1111-4634-b538-a01709473333"),
                    ShiftID= new Guid("B5FEDC70-D3A0-4806-BCF4-D1A30CE904CA")
                },
            };
            context.MstEmployee.AddRange(employee);
            context.SaveChanges();
        }
    }
}
