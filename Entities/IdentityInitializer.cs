using System;
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
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "SuperAdmin", ClaimValue = "True" });
                await _roleMgr.CreateAsync(role);
            }

            var adminUser = new ESPLUser()
            {
                UserName = "espladmin",
                FirstName = "ESPL",
                LastName = "Admin",
                Email = "espl.admin@eternussolutions.com"
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
                await AddAdminUser();
                await AddManagerUser();
                await AddEmployeeUser();
            }
        }
    }
}