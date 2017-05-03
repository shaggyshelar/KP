using ESPL.KP.Models;
using ESPL.KP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESPL.KP.Helpers;
using AutoMapper;
using ESPL.KP.Entities;
using Microsoft.AspNetCore.Http;
using ESPL.KP.Models.Core;
using ESPL.KP.Helpers.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ESPL.KP.Controllers.Core
{
    [Route("api/appusers/{userId}/roles")]
    [Authorize(Policy = "IsSuperAdmin")]
    public class AppUserRoleController : Controller
    {
        private ILibraryRepository _libraryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<AppUser> _userMgr;


        public AppUserRoleController(ILibraryRepository libraryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService,
            UserManager<AppUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _libraryRepository = libraryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        [HttpGet(Name = "GetUserRoles")]
        [HttpHead]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var userFromDB = _libraryRepository.GetAppUser(userId);
            if (userFromDB == null)
            {
                return NotFound("User Not Found");
            }

            var roleList = new List<IdentityRole>();
            var rolesFromDB = await _userMgr.GetRolesAsync(userFromDB);
            return Ok(rolesFromDB);
        }


        [HttpPost(Name = "AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(Guid userId,
            [FromBody] AddUserToRoleDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userFromDB = _libraryRepository.GetAppUser(userId);
            if (userFromDB == null)
            {
                return NotFound("User Not Found");
            }


            var roleFromDB = _libraryRepository.GetAppRole(user.RoleId);
            if (roleFromDB == null)
            {
                return NotFound("Role Not Found");
            }


            await _userMgr.AddToRoleAsync(userFromDB, roleFromDB.Name);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Creating a book for author {userId} failed on save.");
            }

            return Ok();
        }

        [HttpDelete(Name = "RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(Guid userId,
            [FromBody] AddUserToRoleDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userFromDB = _libraryRepository.GetAppUser(userId);
            if (userFromDB == null)
            {
                return NotFound("User Not Found");
            }


            var roleFromDB = _libraryRepository.GetAppRole(user.RoleId);
            if (roleFromDB == null)
            {
                return NotFound("Role Not Found");
            }


            await _userMgr.RemoveFromRoleAsync(userFromDB, roleFromDB.Name);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Removing user from role failed on save.");
            }

            return Ok();
        }

    }
}