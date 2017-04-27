using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESPL.KP.Entities
{
    public class IdentityInitializer
    {
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<ESPLUser> _userMgr;

        public IdentityInitializer(UserManager<ESPLUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task AddAdminUser()
        {
            if (!(await _roleMgr.RoleExistsAsync("SystemAdmin")))
            {
                var role = new IdentityRole("SystemAdmin");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "SystemAdmin", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var adminUser = new ESPLUser()
            {
                UserName = "espladmin",
                FirstName = "ESPL",
                LastName = "SystemAdmin",
                Email = "espl.admin@eternussolutions.com"
            };

            var adminUserResult = await _userMgr.CreateAsync(adminUser, "Espl@123");
            var adminRoleResult = await _userMgr.AddToRoleAsync(adminUser, "SystemAdmin");

            if (!adminUserResult.Succeeded || !adminRoleResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to build user and roles");
            }
        }

        public async Task AddManagerUser()
        {
            if (!(await _roleMgr.RoleExistsAsync("Manager")))
            {
                var role = new IdentityRole("Manager");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsManager", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var user = new ESPLUser()
            {
                UserName = "esplmanager",
                FirstName = "ESPL",
                LastName = "Manager",
                Email = "espl.manager@eternussolutions.com"
            };

            var userResult = await _userMgr.CreateAsync(user, "Espl@123");
            var roleResult = await _userMgr.AddToRoleAsync(user, "Manager");

            if (!userResult.Succeeded || !roleResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to build user and roles");
            }
        }

        public async Task AddEmployeeUser()
        {
            if (!(await _roleMgr.RoleExistsAsync("Employee")))
            {
                var role = new IdentityRole("Employee");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsEmployee", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "DP.C", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "DP.R", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "DP.U", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "DP.D", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var user = new ESPLUser()
            {
                UserName = "esplemployee",
                FirstName = "ESPL",
                LastName = "Employee",
                Email = "espl.employee@eternussolutions.com"
            };

            var userResult = await _userMgr.CreateAsync(user, "Espl@123");
            var roleResult = await _userMgr.AddToRoleAsync(user, "Employee");

            if (!userResult.Succeeded || !roleResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to build user and roles");
            }
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("espladmin");

            // Add User
            if (user == null)
            {
                // await AddAdminUser();
                // await AddManagerUser();
                // await AddEmployeeUser();

                // await AddAllEmployees();
                // await AddAllManagers();
                // await AddAllAdmins();

                await AddAllConstables();
                await AddAllSergeant();
                await AddAllSAIG();
                await AddAllDIG();
                await AddAllIG();
                await AddAllASP();
                await AddAllSuperAdmins();

            }
        }

        public async Task AddAllEmployees()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    UserName = "esplemployee",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7301",
                    FirstName = "ESPL",
                    LastName = "Employee",
                    Email = "espl.employee@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee1",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7302",
                    FirstName = "ESPL 1",
                    LastName = "Employee 1",
                    Email = "espl.employee1@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee2",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7303",
                    FirstName = "ESPL 2",
                    LastName = "Employee 2",
                    Email = "espl.employee2@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee3",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7304",
                    FirstName = "ESPL 3",
                    LastName = "Employee 3",
                    Email = "espl.employee3@eternussolutions.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("Employee")))
            {
                var role = new IdentityRole("Employee");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsEmployee",
                    ClaimValue = "True"
                });
                await _roleMgr.CreateAsync(role);
            }

            foreach (ESPLUser user in allUsers)
            {
                var userResult = await _userMgr.CreateAsync(user, "Espl@123");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Employee");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

        }

        public async Task AddAllManagers()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>() {
                new ESPLUser() {
                    UserName = "esplmanager",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7201",
                    FirstName = "ESPL 1",
                    LastName = "Manager",
                    Email = "espl.manager1@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager1",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7202",
                    FirstName = "ESPL 2",
                    LastName = "Manager",
                    Email = "espl.manager2@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager2",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7203",
                    FirstName = "ESPL 3",
                    LastName = "Manager",
                    Email = "espl.manager3@eternussolutions.com"
                }

            };

            if (!(await _roleMgr.RoleExistsAsync("Manager")))
            {
                var role = new IdentityRole("Manager");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsManager",
                    ClaimValue = "True"
                });
                await _roleMgr.CreateAsync(role);
            }

            foreach (var user in allUsers)
            {
                var userResult = await _userMgr.CreateAsync(user, "Espl@123");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Manager");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }

        public async Task AddAllAdmins()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>() {
                new ESPLUser() {
                    UserName = "espladmin",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7101",
                    FirstName = "ESPL",
                    LastName = "SystemAdmin",
                    Email = "espl.admin@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "espladmin1",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7102",
                    FirstName = "ESPL1",
                    LastName = "Admin1",
                    Email = "espl.admin1@eternussolutions.com"
                }

            };

            if (!(await _roleMgr.RoleExistsAsync("SystemAdmin")))
            {
                var role = new IdentityRole("SystemAdmin");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "SystemAdmin",
                    ClaimValue = "True"
                });
                await _roleMgr.CreateAsync(role);
            }

            foreach (var adminUser in allUsers)
            {
                var adminUserResult = await _userMgr.CreateAsync(adminUser, "Espl@123");
                var adminRoleResult = await _userMgr.AddToRoleAsync(adminUser, "SystemAdmin");

                if (!adminUserResult.Succeeded || !adminRoleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

        }

        public async Task AddAllConstables()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7301",
                    UserName = "tonystark",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "tony.stark@kenyapolice.com"
                },new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7302",
                    UserName = "steverogers",
                    FirstName = "Steve",
                    LastName = "Rogers",
                    Email = "steve.rogers@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("Constable")))
            {
                var role = new IdentityRole("Constable");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsConstable",
                    ClaimValue = "True"
                });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Update", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "Constable", "Espl@123");

        }

        public async Task AddAllSergeant()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7303",
                    UserName = "bradpitt",
                    FirstName = "Brad",
                    LastName = "Pitt",
                    Email = "brad.pitt@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("Sergeant")))
            {
                var role = new IdentityRole("Sergeant");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsSergeant",
                    ClaimValue = "True"
                });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Add", ClaimValue = "True" });


                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "Sergeant", "Espl@123");
        }

        public async Task AddAllSAIG()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7201",
                    UserName = "angelinajolie",
                    FirstName = "Angelina",
                    LastName = "Jolie",
                    Email = "angelina.jolie@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("SAIG")))
            {
                var role = new IdentityRole("SAIG");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsSAIG",
                    ClaimValue = "True"
                });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Delete", ClaimValue = "True" });

                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "SAIG", "Espl@123");
        }

        public async Task AddAllDIG()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7202",
                    UserName = "jacksparrow",
                    FirstName = "Jack",
                    LastName = "Sparrow",
                    Email = "jack.sparrow@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("DIG")))
            {
                var role = new IdentityRole("DIG");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsDIG",
                    ClaimValue = "True"
                });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Delete", ClaimValue = "True" });

                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "DIG", "Espl@123");
        }

        public async Task AddAllIG()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7203",
                    UserName = "johndoe",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("IG")))
            {
                var role = new IdentityRole("IG");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsIG",
                    ClaimValue = "True"
                });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Delete", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "IG", "Espl@123");
        }

        public async Task AddAllASP()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7204",
                    UserName = "johnydepp",
                    FirstName = "Johny",
                    LastName = "Depp",
                    Email = "johny.depp@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("ASP")))
            {
                var role = new IdentityRole("ASP");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsASP", ClaimValue = "True" });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });


                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });


                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "ASP", "Espl@123");
        }

        public async Task AddAllSuperAdmins()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7101",
                    UserName = "nickjones",
                    FirstName = "Nick",
                    LastName = "Jones",
                    Email = "nick.jones@kenyapolice.com"
                }
            };

            if (!(await _roleMgr.RoleExistsAsync("ADMIN")))
            {
                var role = new IdentityRole("ADMIN");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" });
                //----read
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Read", ClaimValue = "True" });

                //----- write
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designations.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Delete", ClaimValue = "True" });

                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "ADMIN", "Espl@123");
        }

        public async Task AddUserWithRole(List<ESPLUser> allUsers, string roleName, string password)
        {


            foreach (ESPLUser user in allUsers)
            {
                var userResult = await _userMgr.CreateAsync(user, password);
                var roleResult = await _userMgr.AddToRoleAsync(user, roleName);

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }

    }
}
