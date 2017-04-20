using System;
using System.Collections.Generic;

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


            #region Area
            context.MstArea.RemoveRange(context.MstArea);
            context.SaveChanges();
            var areas = new List<MstArea>()
            {
                new MstArea()
                {
                    AreaID = new Guid("C8B91521-8578-401B-A638-B97476F28F3E"),
                    AreaName = "Mumbai",
                    AreaCode = "MUM",
                    PinCode = "M001"
                },
                new MstArea()
                {
                    AreaID = new Guid("758B1995-7F92-4D87-9588-B90800ABF825"),
                    AreaName = "Pune",
                    AreaCode = "PU",
                    PinCode = "P002"
                },
                new MstArea()
                {
                    AreaID = new Guid("497F75A9-8CCE-4085-A872-C14E26820A3F"),
                    AreaName = "Nasik",
                    AreaCode = "NAS",
                    PinCode = "N003"
                }
            };

            //context.MstArea.AddRange(areas);
            //context.SaveChanges();
            #endregion

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

            #region Designations
            context.MstDesignation.RemoveRange(context.MstDesignation);
            context.SaveChanges();
            var designations = new List<MstDesignation>()
            {
                new MstDesignation()
                {
                    DesignationID = new Guid("B7F83929-EAAC-49F8-9A7A-5F5FFC2018C3"),
                    DesignationName = "Inspector",
                    DesignationCode = "INS"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("778E2940-0AB2-4988-90FB-245042A4E24B"),
                    DesignationName = "SubInspector",
                    DesignationCode = "INS"
                },
                new MstDesignation()
                {
                    DesignationID = new Guid("95D1B726-6AE0-473F-AD6B-7FC3059AE472"),
                    DesignationName = "Constable",
                    DesignationCode = "INS"
                }
            };

            context.MstDesignation.AddRange(designations);
            context.SaveChanges();
            #endregion


            UpdateDepartments(context);
            UpdateOccurrenceType(context);
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
    }
}
