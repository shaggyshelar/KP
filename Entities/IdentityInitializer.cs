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
            if (!(await _roleMgr.RoleExistsAsync("Admin")))
            {
                var role = new IdentityRole("Admin");
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var adminUser = new ESPLUser()
            {
                UserName = "espladmin",
                FirstName = "ESPL",
                LastName = "Admin",
                Email = "espl.admin@kenyapolice.com"
            };

            var adminUserResult = await _userMgr.CreateAsync(adminUser, "Espl@123");
            var adminRoleResult = await _userMgr.AddToRoleAsync(adminUser, "Admin");

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
                Email = "espl.manager@kenyapolice.com"
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
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.AddEdit", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Department.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designation.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designation.AddEdit", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designation.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Designation.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Read", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.AddEdit", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var user = new ESPLUser()
            {
                UserName = "esplemployee",
                FirstName = "ESPL",
                LastName = "Employee",
                Email = "espl.employee@kenyapolice.com"
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

                await AddAllConstables();
                await AddAllSergeant();
                await AddAllSAIG();
                await AddAllDIG();
                await AddAllIG();
                await AddAllASP();
            }
        }

        public async Task AddAllConstables()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    UserName = "tonystark",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Email = "tony.stark@kenyapolice.com"
                },new ESPLUser()
                {
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
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Permissions.Delete", ClaimValue = "True" });

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
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });

                //------ Delete
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Area.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Delete", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceType.Delete", ClaimValue = "True" });
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
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Add", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Add", ClaimValue = "True" });

                //----- update
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Employee.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceBook.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Shift.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "Status.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceAssignmentHistory.Update", ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "OccurrenceReviewHistory.Update", ClaimValue = "True" });

                await _roleMgr.CreateAsync(role);
            }

            await AddUserWithRole(allUsers, "ASP", "Espl@123");
        }

        public async Task AddAllManagers()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>() {
                new ESPLUser() {
                    UserName = "esplmanager",
                    FirstName = "ESPL 1",
                    LastName = "Manager",
                    Email = "espl.manager1@kenyapolice.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager1",
                    FirstName = "ESPL 2",
                    LastName = "Manager",
                    Email = "espl.manager2@kenyapolice.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager2",
                    FirstName = "ESPL 3",
                    LastName = "Manager",
                    Email = "espl.manager3@kenyapolice.com"
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
                    FirstName = "ESPL",
                    LastName = "Admin",
                    Email = "espl.admin@kenyapolice.com"
                },
                new ESPLUser() {
                    UserName = "espladmin1",
                    FirstName = "ESPL1",
                    LastName = "Admin1",
                    Email = "espl.admin1@kenyapolice.com"
                }

            };

            if (!(await _roleMgr.RoleExistsAsync("Admin")))
            {
                var role = new IdentityRole("Admin");
                role.Claims.Add(new IdentityRoleClaim<string>()
                {
                    ClaimType = "IsAdmin",
                    ClaimValue = "True"
                });
                await _roleMgr.CreateAsync(role);
            }

            foreach (var adminUser in allUsers)
            {
                var adminUserResult = await _userMgr.CreateAsync(adminUser, "Espl@123");
                var adminRoleResult = await _userMgr.AddToRoleAsync(adminUser, "Admin");

                if (!adminUserResult.Succeeded || !adminRoleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

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