using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KP.Common.Enums;
using KP.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KP.Persistence
{
    public class IdentityInitializer
    {
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<AppUser> _userMgr;

        public IdentityInitializer(UserManager<AppUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed()
        {
            var user = await _userMgr.FindByNameAsync("tomcruise");

            // Add User
            if (user == null)
            {
                await AddAllAdmins();
            }
        }

        public async Task AddAllAdmins()
        {
            List<AppUser> allUsers = new List<AppUser>() {
                new AppUser() {
                    UserName = "tomcruise",
                    Id = "56c385ae-ce46-41d4-b7fe-08df9aef7102",
                    FirstName = "Tom",
                    LastName = "Cruise",
                    Email = "tom.cruise@eternussolutions.com"
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

                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = Permissions.DepartmentCreate, ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = Permissions.DepartmentRead, ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = Permissions.DepartmentUpdate, ClaimValue = "True" });
                role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = Permissions.DepartmentDelete, ClaimValue = "True" });
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