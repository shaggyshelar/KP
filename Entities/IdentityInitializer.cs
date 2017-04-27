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

                await AddAllEmployees();
                await AddAllManagers();
                await AddAllAdmins();
            }
        }

        public async Task AddAllEmployees()
        {
            List<ESPLUser> allUsers = new List<ESPLUser>()
            {
                new ESPLUser()
                {
                    UserName = "esplemployee",
                    FirstName = "ESPL",
                    LastName = "Employee",
                    Email = "espl.employee@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee1",
                    FirstName = "ESPL 1",
                    LastName = "Employee 1",
                    Email = "espl.employee1@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee2",
                    FirstName = "ESPL 2",
                    LastName = "Employee 2",
                    Email = "espl.employee2@eternussolutions.com"
                },new ESPLUser()
                {
                    UserName = "esplemployee3",
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
                    FirstName = "ESPL 1",
                    LastName = "Manager",
                    Email = "espl.manager1@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager1",
                    FirstName = "ESPL 2",
                    LastName = "Manager",
                    Email = "espl.manager2@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "esplmanager2",
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
                    FirstName = "ESPL",
                    LastName = "SystemAdmin",
                    Email = "espl.admin@eternussolutions.com"
                },
                new ESPLUser() {
                    UserName = "espladmin1",
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

    }
}